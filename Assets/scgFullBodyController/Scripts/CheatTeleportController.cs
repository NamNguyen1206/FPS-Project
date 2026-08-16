using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheatTeleportController : MonoBehaviour
{
    [Serializable]
    public class TeleportCheat
    {
        [Tooltip("Phím phụ để kích hoạt teleport. Ví dụ Alpha0, Alpha1, Alpha2...")]
        public KeyCode triggerKey = KeyCode.Alpha0;

        [Tooltip("Điểm teleport tương ứng.")]
        public Transform teleportPoint;
    }

    [Header("Cheat Settings")]
    [SerializeField] private bool cheatEnabled = true;

    [Tooltip("Phím chính để kích hoạt cheat. Ví dụ K.")]
    [SerializeField] private KeyCode activationKey = KeyCode.K;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Teleport Points")]
    [SerializeField] private List<TeleportCheat> teleportCheats =
        new List<TeleportCheat>();

    private void Start()
    {
        // Nếu chưa kéo Player vào Inspector
        // thì tự tìm Player bằng Tag.
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning(
                    "CheatTeleportController: Không tìm thấy Player."
                );
            }
        }
    }

    private void Update()
    {
        if (!cheatEnabled)
            return;

        if (player == null)
            return;

        // Phải giữ phím K
        if (!Input.GetKey(activationKey))
            return;

        // Kiểm tra các phím phụ
        foreach (TeleportCheat cheat in teleportCheats)
        {
            if (cheat == null)
                continue;

            if (cheat.teleportPoint == null)
                continue;

            if (Input.GetKeyDown(cheat.triggerKey))
            {
                TeleportPlayer(cheat.teleportPoint);
                break;
            }
        }
    }

    private void TeleportPlayer(Transform destination)
    {
        if (destination == null)
            return;

        Vector3 targetPosition = destination.position;
        Quaternion targetRotation = destination.rotation;

        // -------------------------------------------------
        // NavMeshAgent
        // -------------------------------------------------

        NavMeshAgent agent =
            player.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.Warp(targetPosition);
            }
            else
            {
                player.position = targetPosition;
            }
        }
        else
        {
            // -------------------------------------------------
            // CharacterController
            // -------------------------------------------------

            CharacterController characterController =
                player.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false;

                player.SetPositionAndRotation(
                    targetPosition,
                    targetRotation
                );

                characterController.enabled = true;
            }
            else
            {
                player.SetPositionAndRotation(
                    targetPosition,
                    targetRotation
                );
            }
        }

        // -------------------------------------------------
        // Rigidbody
        // -------------------------------------------------

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log(
            $"CHEAT TELEPORT: {player.name} → {destination.name}"
        );
    }
}
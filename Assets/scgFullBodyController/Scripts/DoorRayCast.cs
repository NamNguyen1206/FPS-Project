using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DoorRayCast : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TextMeshProUGUI interactionText;
    public float interactDistance = 5f;
    private KeyInventory inventory;
    private ArmorPickup currentArmor;

    private void Start()
    {
        inventory = GetComponent<KeyInventory>();
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    private void Update()
    {
        currentArmor = null;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            DoorController door = hit.collider.GetComponentInParent<DoorController>();
            MystoryBox mysteryBox = hit.collider.GetComponentInParent<MystoryBox>();
            ArmorPickup armor = hit.collider.GetComponentInParent<ArmorPickup>();
            GrenadePickup grenade = hit.collider.GetComponentInParent<GrenadePickup>();
            KeyPickup key = hit.collider.GetComponentInParent<KeyPickup>();
            SyringePickup syringe = hit.collider.GetComponentInParent<SyringePickup>();
            NPCDialogue npc = hit.collider.GetComponentInParent<NPCDialogue>();

            // ===== HIỆN UI CHO ARMOR =====

            // =========================
            // DOOR
            // =========================

            if (door != null)
            {
                if (inventory != null && inventory.hasKey)
                {
                    // Có key
                    ShowInteractionText("[E] Open Door");
                }
                else
                {
                    // Không có key
                    ShowInteractionText("Need to find the key");
                }
            }

            // =========================
            // ARMOR
            // =========================

            else if (armor != null)
            {
                currentArmor = armor;

                ShowInteractionText("[E] Pick Up Armor");
            }

            // =========================
            // GRENADE
            // =========================

            else if (grenade != null)
            {
                ShowInteractionText("[E] Pick Up Grenade");
            }

            // =========================
            // KEY
            // =========================

            else if (key != null)
            {
                ShowInteractionText("[E] Pick Up Key");
            }

            // =========================
            // SYRINGE
            // =========================

            else if (syringe != null)
            {
                ShowInteractionText("[E] Pick Up Syringe");
            }

            // =========================
            // NPC
            // =========================

            else if (npc != null)
            {
                // Nếu NPC chưa nói chuyện
                if (!npc.IsTalking)
                {
                    ShowInteractionText("[E] to talk to NPC");
                }
                else
                {
                    // Đang dialogue -> không hiện interaction UI
                    HideInteractionUI();
                }
            }

            // =========================
            // NOTHING
            // =========================

            else
            {
                HideInteractionUI();
            }

            // ===== NHẤN E ĐỂ TƯƠNG TÁC =====

            if (Input.GetKeyDown(KeyCode.E))
            {
                // DOOR
                if (door != null)
                {
                    if (inventory != null && inventory.hasKey)
                    {
                        door.Interact();

                        HideInteractionUI();
                    }
                    else
                    {
                        Debug.Log("Door Locked - Need Key");
                    }
                }

                // MYSTERY BOX
                else if (mysteryBox != null)
                {
                    mysteryBox.Interact();

                    HideInteractionUI();
                }

                // ARMOR
                else if (currentArmor != null)
                {
                    currentArmor.Interact();

                    HideInteractionUI();
                }

                // GRENADE
                else if (grenade != null)
                {
                    grenade.Interact();

                    HideInteractionUI();
                }

                // KEY
                else if (key != null)
                {
                    key.Interact();

                    HideInteractionUI();
                }

                // SYRINGE
                else if (syringe != null)
                {
                    syringe.Interact();

                    HideInteractionUI();
                }

                // NPC
                else if (npc != null)
                {
                    npc.NPCInteract();

                    // Ẩn "[E] to talk to NPC"
                    HideInteractionUI();
                }
            }
        }
        else
        {
            HideInteractionUI();
        }
    }
    private void ShowInteractionText(string text)
    {
        if (interactionUI != null)
            interactionUI.SetActive(true);

        if (interactionText != null)
            interactionText.text = text;
    }

    private void HideInteractionUI()
    {
        currentArmor = null;

        if (interactionUI != null)
            interactionUI.SetActive(false);
    }
}

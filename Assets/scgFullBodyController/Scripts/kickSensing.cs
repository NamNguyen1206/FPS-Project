//SlapChickenGames
//2021
//Kick leg sensing 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class kickSensing : MonoBehaviour
    {
        public float playerKickforce;
        public float doorKickforce;
        public GameObject cameraObj;
        public AudioClip kickSound;
        public int kickDamage;

        void OnTriggerEnter(Collider col)
        {
            Animator playerAnimator = transform.root.GetComponent<Animator>();
            bool isKicking = playerAnimator != null && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Kick");

            if (!isKicking)
                return;

            //If we hit a player, apply damage to the player transform root object's health controller
            if (col.CompareTag("Player") && col.transform.root.GetComponent<HealthController>())
            {
                col.transform.root.GetComponent<HealthController>().DamageByKick(cameraObj.transform.forward * 360, playerKickforce, kickDamage);
                PlayKickSound();
            }

            // Damage the zombie root, even when the kick hits one of its child colliders.
            ZombieController zombie = col.GetComponentInParent<ZombieController>();
            if (zombie != null)
            {
                zombie.TakeDamage(kickDamage);
                PlayKickSound();
            }

            //If we hit a door, add force to its rigidbody
            if (col.CompareTag("Door"))
            {
                Rigidbody doorRigidbody = col.GetComponent<Rigidbody>();
                if (doorRigidbody != null)
                {
                    doorRigidbody.AddForce(cameraObj.transform.forward * 360 * doorKickforce);
                    PlayKickSound();
                }
            }
        }

        private void PlayKickSound()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && kickSound != null)
                audioSource.PlayOneShot(kickSound);
        }
    }
}

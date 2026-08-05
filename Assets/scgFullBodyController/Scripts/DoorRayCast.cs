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

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    //         RaycastHit hit;

    //         if (Physics.Raycast(ray,out hit,interactDistance))
    //         {
    //             Debug.Log("Hit: " + hit.collider.name);
    //             DoorController door = hit.collider.GetComponentInParent<DoorController>();
    //             MystoryBox mysteryBox = hit.collider.GetComponentInParent<MystoryBox>();
    //             ArmorPickup armor = hit.collider.GetComponentInParent<ArmorPickup>();

    //             if (door != null)
    //             {
    //                 if (inventory != null &&
    //                     inventory.hasKey)
    //                 {
    //                     door.Interact();
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("Door Locked - Need Hangar Key");
    //                 }
    //             }
    //             else if (mysteryBox != null)
    //             {
    //                 Debug.Log("MystoryBox found: " + mysteryBox.name);
    //                 mysteryBox.Interact();
    //             }
    //             else if (armor != null)
    //             {
    //                 Debug.Log("Armor found: " + armor.name);
    //                 armor.Interact();
    //             }
    //             else
    //             {
    //                 Debug.Log("No interactable found on hit object");
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("Raycast missed");
    //         }
    //     }
    // }
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

            // ===== HIỆN UI CHO ARMOR =====
            if (armor != null)
            {
                currentArmor = armor;

                if (interactionUI != null)
                    interactionUI.SetActive(true);
                if (interactionText != null)
                    interactionText.text = "[E] Pick Up Armor";
            }
            else if (grenade != null)
            {
                if (interactionUI != null)
                    interactionUI.SetActive(true);
                if (interactionText != null)
                    interactionText.text = "[E] Pick Up Grenade";
            }
            else if (key != null)
            {
                if (interactionUI != null)
                    interactionUI.SetActive(true);

                if (interactionText != null)
                    interactionText.text = "[E] Pick Up Key";
            }
            else if (syringe != null)
            {
                if (interactionUI != null)
                    interactionUI.SetActive(true);

                if (interactionText != null)
                    interactionText.text = "[E] Pick Up Syringe";
            }
            else
            {
                currentArmor = null;
                if (interactionUI != null)
                    interactionUI.SetActive(false);
            }

            // ===== NHẤN E ĐỂ TƯƠNG TÁC =====
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (door != null)
                {
                    if (inventory != null && inventory.hasKey)
                        door.Interact();
                    else
                        Debug.Log("Door Locked - Need Hangar Key");
                }
                else if (mysteryBox != null)
                {
                    mysteryBox.Interact();
                }
                else if (currentArmor != null)
                {
                    currentArmor.Interact();

                    if (interactionUI != null)
                        interactionUI.SetActive(false);
                }
                else if (grenade != null)
                {
                    grenade.Interact();

                    if (interactionUI != null)
                        interactionUI.SetActive(false);
                }
                else if (key != null)
                {
                    key.Interact();

                    if (interactionUI != null)
                        interactionUI.SetActive(false);
                }
                else if (syringe != null)
                {
                    syringe.Interact();

                    if (interactionUI != null)
                        interactionUI.SetActive(false);
                }
            }
        }
        else
        {
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }
}

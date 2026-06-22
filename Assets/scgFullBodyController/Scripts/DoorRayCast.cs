using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorRayCast : MonoBehaviour
{
    public float interactDistance = 5f;
    private KeyInventory inventory;

    private void Start()
    {
        inventory = GetComponent<KeyInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            RaycastHit hit;

            if (Physics.Raycast(ray,out hit,interactDistance))
            {
                //Debug.Log("Hit: " + hit.collider.name);
                DoorController door = hit.collider.GetComponent<DoorController>();

                if (door != null)
                {
                    if (inventory != null &&
                        inventory.hasKey)
                    {
                        door.Interact();
                    }
                    else
                    {
                        Debug.Log("Door Locked - Need Hangar Key");
                    }
                }
            }
        }
    }
}

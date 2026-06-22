using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyInventory inventory =
                other.GetComponent<KeyInventory>();

            if (inventory != null)
            {
                inventory.hasKey = true;

                Debug.Log("Hangar Key Collected!");

                Destroy(gameObject);
            }
        }
    }
}

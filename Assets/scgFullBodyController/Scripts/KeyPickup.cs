using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // [Header("Mission")]
        // [SerializeField]  string missionTitle = "MISSION UPDATED";
        // [SerializeField]  string missionDescription = "Find the Exit";
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

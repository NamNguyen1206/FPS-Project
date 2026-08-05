using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f, 1f)] private float pickupVolume = 0.7f;
//  [SerializeField] private ObjectiveMarker objectiveMarker;

    private bool collected = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         KeyInventory inventory =
    //             other.GetComponent<KeyInventory>();

    //         if (inventory != null)
    //         {
    //             inventory.hasKey = true;

    //             Debug.Log("Hangar Key Collected!");
    //             // Phát âm thanh
    //             if (pickupSound != null)
    //             {
    //                 AudioSource.PlayClipAtPoint(
    //                     pickupSound,
    //                     transform.position,
    //                     pickupVolume
    //                 );
    //             }

    //             Destroy(gameObject);
    //         }
    //     }
    // }
    public void Interact()
    {
        if (collected)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        KeyInventory inventory = player.GetComponent<KeyInventory>();

        if (inventory != null)
        {
            inventory.hasKey = true;

            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.npcMarker.SetActive(true);
                Debug.Log(ObjectiveManager.Instance.npcMarker.name);
                Debug.Log(ObjectiveManager.Instance.npcMarker.activeSelf);
            }

            collected = true;

            Debug.Log("Hangar Key Collected!");

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    pickupSound,
                    transform.position,
                    pickupVolume
                );
            }

            Destroy(gameObject);
        }
    }
}

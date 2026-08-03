using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] [Range(0f, 1f)] private float pickupVolume = 0.7f;

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
                // Phát âm thanh
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
}

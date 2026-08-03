using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    [Header("Pickup")]
    public int amount = 1;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f, 1f)] private float pickupVolume = 0.7f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GrenadeInventory inventory =
            other.GetComponent<GrenadeInventory>();

        if (inventory != null)
        {
            inventory.AddGrenade(amount);
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

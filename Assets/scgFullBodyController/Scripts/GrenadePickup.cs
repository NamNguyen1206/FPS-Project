using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    [Header("Pickup")]
    public int amount = 1;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f, 1f)] private float pickupVolume = 0.7f;
    private bool collected = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (!other.CompareTag("Player"))
    //         return;

    //     GrenadeInventory inventory =
    //         other.GetComponent<GrenadeInventory>();

    //     if (inventory != null)
    //     {
    //         inventory.AddGrenade(amount);
    //         // Phát âm thanh
    //         if (pickupSound != null)
    //         {
    //             AudioSource.PlayClipAtPoint(
    //                 pickupSound,
    //                 transform.position,
    //                 pickupVolume
    //             );
    //         }

    //         Destroy(gameObject);
    //     }
    // }
    public void Interact()
    {
        if (collected)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        GrenadeInventory inventory = player.GetComponent<GrenadeInventory>();

        if (inventory == null)
            return;

        inventory.AddGrenade(amount);

        collected = true;

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position,
                pickupVolume);
        }

        gameObject.SetActive(false);
    }
}

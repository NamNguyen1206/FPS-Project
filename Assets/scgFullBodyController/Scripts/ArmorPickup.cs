using UnityEngine;
using scgFullBodyController;

public class ArmorPickup : MonoBehaviour
{
    [SerializeField] private float armorAmount = 50f;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f,1f)] private float pickupVolume = 0.5f;

    private bool collected = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (collected) return;

    //     HealthController health = other.GetComponent<HealthController>();
    //     if (health != null)
    //     {
    //         health.AddArmor(armorAmount);
    //         collected = true;
    //         // Phát âm thanh
    //         AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
    //         gameObject.SetActive(false);
    //     }
    // }
    public void Interact()
    {
        if (collected)
            return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        HealthController health = player.GetComponent<HealthController>();

        if (health == null)
            return;
        
            health.AddArmor(armorAmount);
            collected = true;
        
        // Phát âm thanh
        AudioSource.PlayClipAtPoint(
            pickupSound,
            transform.position,
            pickupVolume);

        gameObject.SetActive(false);
    }
}

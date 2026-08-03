using UnityEngine;

public class SyringePickup : MonoBehaviour
{
    public BossSpawnController bossSpawner;

    [Header("Mission")]
    [SerializeField] private string missionTitle = "MISSION UPDATED";
    [SerializeField] private string missionDescription = "Find the Exit";

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f, 1f)]private float pickupVolume = 0.7f;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //         //if(bossSpawner != null)
    //         bossSpawner.ActivateBossSpawn();
    //         Destroy(gameObject);
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Hiện thông báo nhiệm vụ
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ShowMission(
                missionTitle,
                missionDescription
            );
        }

        // Kích hoạt Boss Spawn
        if (bossSpawner != null)
        {
            bossSpawner.ActivateBossSpawn();
        }

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
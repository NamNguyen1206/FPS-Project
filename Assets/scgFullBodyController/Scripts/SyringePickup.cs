using UnityEngine;

public class SyringePickup : MonoBehaviour
{
    public BossSpawnController bossSpawner;

    [Header("Mission")]
    [SerializeField] private string missionTitle = "MISSION UPDATED";
    [SerializeField] private string missionDescription = "Find the Exit";

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

        Destroy(gameObject);
    }
}
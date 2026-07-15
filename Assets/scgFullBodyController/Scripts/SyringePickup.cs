using UnityEngine;

public class SyringePickup : MonoBehaviour
{
    public BossSpawnController bossSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //if(bossSpawner != null)
            bossSpawner.ActivateBossSpawn();
            Destroy(gameObject);
        }
    }
}
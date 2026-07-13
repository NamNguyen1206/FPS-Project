using UnityEngine;
using System.Collections;

public class BossSpawnController : MonoBehaviour
{
[Header("Zombie Prefabs")]
    public GameObject[] zombiePrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnDelay = 10f;
    public int zombieCount = 5;
    public float spawnRadius = 4f;

    private void Start()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        SpawnZombies();
    }

    private void SpawnZombies()
    {
        if (zombiePrefabs.Length == 0)
        {
            Debug.LogWarning("No Zombie Prefabs assigned!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No Spawn Points assigned!");
            return;
        }

        for (int i = 0; i < zombieCount; i++)
        {
            int randomSpawnIndex =
                Random.Range(0, spawnPoints.Length);

            Transform spawnPoint =
                spawnPoints[randomSpawnIndex];

            GameObject randomZombiePrefab =
                zombiePrefabs[
                    Random.Range(0, zombiePrefabs.Length)
                ];

            Vector2 randomOffset =
                Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition =
                spawnPoint.position +
                new Vector3(
                    randomOffset.x,
                    0f,
                    randomOffset.y
                );

            Instantiate(
                randomZombiePrefab,
                spawnPosition,
                spawnPoint.rotation
            );
        }

        Debug.Log($"Spawned {zombieCount} zombies.");
    }
}

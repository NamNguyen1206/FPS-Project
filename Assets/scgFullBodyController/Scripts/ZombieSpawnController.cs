using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ZombieSpawnController : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;
    public float timeBetweenWave = 10f;
    [SerializeField] private float waveTimer = 0f;
    private int waveNumber = 1;
    public int zombiePerWave = 4;

    void StartNewWave()
    {
        waveTimer = 0f;
        zombiePerWave +=2;
        float minDistance = 4f;
        for (int i = 0; i < zombiePerWave; i++)
        {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomSpawnIndex];

            GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

            Vector3 spawnPosition = spawnPoint.position + Random.insideUnitSphere * minDistance;
            
            spawnPosition.y = spawnPoint.position.y;
            
            Instantiate(randomZombiePrefab, spawnPosition, spawnPoint.rotation);
        }
        waveNumber++;
    }
}

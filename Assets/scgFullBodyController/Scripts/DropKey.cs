using UnityEngine;

public class DropKey : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform dropPoint;

    private bool hasDropped = false;

    public void ZombieDropKey()
    {
        if (hasDropped)
            return;

        hasDropped = true;

        Vector3 spawnPos = transform.position;

        if (dropPoint != null)
        {
            spawnPos = dropPoint.position;
        }

        Instantiate(keyPrefab, spawnPos, Quaternion.identity);

        Debug.Log("Key Dropped!");
    }
}

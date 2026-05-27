using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 6;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            pool.Enqueue(CreateInstance());
        }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject instance = pool.Count > 0 ? pool.Dequeue() : CreateInstance();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);

        PooledObject pooledObject = instance.GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnSpawn(this);
        }

        return instance;
    }

    public void Release(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        pool.Enqueue(instance);
    }

    private GameObject CreateInstance()
    {
        GameObject instance = prefab != null
            ? Instantiate(prefab, transform)
            : CreateDefaultEffect();

        instance.transform.SetParent(transform);
        instance.SetActive(false);
        return instance;
    }

    private GameObject CreateDefaultEffect()
    {
        GameObject instance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        instance.name = "Pooled Attack Effect";
        instance.transform.localScale = Vector3.one * 0.35f;

        Collider effectCollider = instance.GetComponent<Collider>();
        if (effectCollider != null)
        {
            Destroy(effectCollider);
        }

        Renderer renderer = instance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }

        instance.AddComponent<PooledObject>();
        return instance;
    }
}

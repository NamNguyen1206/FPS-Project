using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.35f;

    private SimpleObjectPool owner;
    private float releaseTime;

    public void OnSpawn(SimpleObjectPool pool)
    {
        owner = pool;
        releaseTime = Time.time + lifeTime;
    }

    private void Update()
    {
        if (owner != null && Time.time >= releaseTime)
        {
            owner.Release(gameObject);
        }
    }
}

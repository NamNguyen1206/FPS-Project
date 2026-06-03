using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject Barrel, Explosion;

    private AudioSource source;
    private Collider[] barrelColliders;
    private bool exploded;
    private int currentHits;

    [SerializeField]
    private float range;

    [SerializeField]
    private int hitsToExplode = 3;

    private void Awake()
    {
        Barrel.SetActive(true);
        Explosion.SetActive(false);

        source = GetComponent<AudioSource>();
        barrelColliders = GetComponentsInChildren<Collider>();
    }

    public void TakeHit()
    {
        if (exploded)
        {
            return;
        }

        currentHits++;

        if (currentHits >= hitsToExplode)
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (exploded)
        {
            return;
        }

        exploded = true;

        foreach (Collider barrelCollider in barrelColliders)
        {
            barrelCollider.enabled = false;
        }

        Barrel.SetActive(false);
        Explosion.SetActive(true);

        if (source != null)
        {
            source.Play();
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();

            if (enemyComponent != null)
            {
                enemyComponent.KillEnemy(transform.position);
            }
        }

        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

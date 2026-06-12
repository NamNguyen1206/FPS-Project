using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [Header("Stats")]
    public int ZombieDamage = 20;
    [SerializeField] private int HP = 100;

    [Header("References")]
    public bool useRagdoll = false;
    public float destroyAfterDeath = 10f;
    private Collider[] RagdollColliders;

    [Header("References")]
    public ZombieHand zombieHand;
    private Animator anim;
    private NavMeshAgent navAgent;
    private Collider MonsterCollider;


    private bool isDead = false;
    
    private void Awake()
    {
        MonsterCollider = GetComponent<Collider>();
        RagdollColliders = GetComponentsInChildren<Collider>();
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        //ActivateRagdoll(false);
        if (useRagdoll)
        {
            ActivateRagdoll(false);
        }
    }

    private void Start()
    {
        if (zombieHand == null)
        {
            zombieHand = GetComponentInChildren<ZombieHand>();
        }

        if (zombieHand != null)
        {
            zombieHand.damage = ZombieDamage;
        }
        else
        {
            Debug.LogWarning("ZombieHand not found!", this);
        }
    }
    
    public void TakeDamage(int damageAmount)
    {
        if (isDead)
        return;

        HP -= damageAmount;
        Debug.Log($"{name} took {damageAmount} damage. HP: {HP}");

        if(HP <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Damage");
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (navAgent != null)
        {
            navAgent.isStopped = true;
            navAgent.enabled = false;
        }

        if (zombieHand != null)
        {
            zombieHand.enabled = false;
        }

        int randomDeath = Random.Range(0, 2);

        if (!useRagdoll)
        {
            if (randomDeath == 0)
                anim.SetTrigger("Die1");
            else
                anim.SetTrigger("Die2");

            Destroy(gameObject, destroyAfterDeath);
        }
        else
        {
            ActivateRagdoll(true);
            Destroy(gameObject, destroyAfterDeath);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);// Attack Range

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 18f);// Detection Range

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 20f);// Stop Chasing Range
    }

    private void ActivateRagdoll(bool status)
    {
        if (RagdollColliders == null)
            return;

        foreach (Collider col in RagdollColliders)
        {
            if (col == MonsterCollider)
                continue;

            col.enabled = status;

            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = !status;
            }
        }

        if (MonsterCollider != null)
            MonsterCollider.enabled = !status;

        if (anim != null)
            anim.enabled = !status;
    }

    public void KillEnemy(Vector3 ExplosionPosition)
    {
        //ActivateRagdoll(true);
        if (isDead)
        return;

        Die();

        if (!useRagdoll)
        return;

        foreach (Collider col in RagdollColliders)
        {
            //col.GetComponent<Rigidbody>().AddExplosionForce(40f, ExplosionPosition, 3f, 3f, ForceMode.VelocityChange);
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(40f,ExplosionPosition,3f,3f,ForceMode.VelocityChange
                );
            }
        }
    }
}

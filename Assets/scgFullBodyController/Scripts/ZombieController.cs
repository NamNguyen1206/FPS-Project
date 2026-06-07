using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Collider MonsterCollider;
    private Collider[] RagdollColliders;
    public ZombieHand zombieHand;
    public int ZombieDamage = 20;
    private Animator anim;
    private NavMeshAgent navAgent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

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
            Debug.LogWarning("ZombieHand is not assigned and could not be found in children.", this);
        }
    }
    private void Awake()
    {
        //MonsterCollider = GetComponent<Collider>();
        //RagdollColliders = GetComponentsInChildren<Collider>();
        anim = GetComponentInChildren<Animator>();

        //ActivateRagdoll(false);
    }

    private void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if(HP <= 0)
        {
            int randomDeath = Random.Range(0, 2);
            if(randomDeath == 0)
            {
                anim.SetTrigger("Die1");
            }
            else
            {
                anim.SetTrigger("Die2");
            }
        }
        else
        {
            anim.SetTrigger("Damage");
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

    //private void ActivateRagdoll(bool Status)
    //{
        //foreach (Collider col in RagdollColliders)
        //{
            //col.enabled = Status;
        //}

        //MonsterCollider.enabled = !Status;
        //anim.enabled = !Status;
        //GetComponent<Rigidbody>().useGravity = !Status;
    //}

    public void KillEnemy(Vector3 ExplosionPosition)
    {
        //ActivateRagdoll(true);

        foreach (Collider col in RagdollColliders)
        {
            col.GetComponent<Rigidbody>().AddExplosionForce(40f, ExplosionPosition, 3f, 3f, ForceMode.VelocityChange);
        }
    }
}

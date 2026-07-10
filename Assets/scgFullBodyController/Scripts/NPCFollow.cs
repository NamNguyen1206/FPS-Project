using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Follow")]
    public GameObject ObjectToFollow;
    public bool canFollow = false;

    [Header("Zombie Detection")]
    public float detectZombieRadius = 8f;

    private bool isTerrified = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ObjectToFollow == null)
            return;

        //-----------------------
        // Toggle Follow
        //-----------------------

        if (Input.GetKeyDown(KeyCode.X))
        {
            canFollow = !canFollow;

            if (!canFollow)
            {
                agent.isStopped = true;
                SetAnimation(0);

                Debug.Log("NPC Stop Following");
            }
            else
            {
                Debug.Log("NPC Start Following");
            }
        }

        //-----------------------
        // Detect Zombie
        //-----------------------

        if (DetectZombie())
        {
            //Debug.Log("Zombie Detected!");
            if (!isTerrified)
            {
                isTerrified = true;

                agent.isStopped = true;

                SetAnimation(3);

                Debug.Log("Zombie Detected");
            }

            return;
        }
        else
        {
            if (isTerrified)
            {
                isTerrified = false;
                agent.isStopped = false;

                Debug.Log("Zombie Gone");
            }
        }

        //-----------------------
        // Follow OFF
        //-----------------------

        if (!canFollow)
            return;

        float distance = Vector3.Distance(
            transform.position,
            ObjectToFollow.transform.position);

        if (distance < 3)
        {
            agent.isStopped = true;
            SetAnimation(0);
        }
        else if (distance >= 3 && distance < 10)
        {
            agent.isStopped = false;
            agent.speed = 2;
            agent.SetDestination(ObjectToFollow.transform.position);

            SetAnimation(1);
        }
        else if (distance >= 10)
        {
            agent.isStopped = false;
            agent.speed = 6;
            agent.SetDestination(ObjectToFollow.transform.position);

            SetAnimation(2);
        }
    }

    bool DetectZombie()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,detectZombieRadius);
        //Debug.Log("Found: " + hits.Length);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Zombie"))
                return true;
        }

        return false;
    }

    void SetAnimation(int state)
    {
        animator.SetInteger("C", state);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectZombieRadius);
    }
}
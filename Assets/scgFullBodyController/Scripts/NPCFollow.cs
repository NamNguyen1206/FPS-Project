using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    [Header("Zombie Spawn")]
    public GameObject zombieSpawnObject;

    private bool hasActivatedSpawn = false;

    // ============================================================
    // REFERENCES
    // ============================================================

    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead = false;

    // ============================================================
    // FOLLOW
    // ============================================================

    [Header("Follow")]
    public GameObject ObjectToFollow;

    public bool canFollow = false;

    // ============================================================
    // ZOMBIE DETECTION
    // ============================================================

    [Header("Zombie Detection")]
    public float detectZombieRadius = 8f;

    private bool isTerrified = false;

    // ============================================================
    // BAD END
    // ============================================================

    [Header("Bad End")]
    public GameObject badEndText;

    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError(
                $"NPC [{name}] does not have NavMeshAgent!",
                this
            );
        }

        if (animator == null)
        {
            Debug.LogError(
                $"NPC [{name}] does not have Animator!",
                this
            );
        }
    }

    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // NPC chết thì không xử lý logic nữa
        if (isDead)
            return;

        if (ObjectToFollow == null)
            return;

        // ========================================================
        // TOGGLE FOLLOW
        // ========================================================

        if (Input.GetKeyDown(KeyCode.X))
        {
            canFollow = !canFollow;

            if (!canFollow)
            {
                StopNPC();

                SetAnimation(0);

                Debug.Log(
                    "NPC Stop Following"
                );
            }
            else
            {
                // Kích hoạt Zombie Spawn
                if (
                    !hasActivatedSpawn &&
                    zombieSpawnObject != null
                )
                {
                    zombieSpawnObject.SetActive(true);

                    hasActivatedSpawn = true;
                }

                Debug.Log(
                    "NPC Start Following"
                );
            }
        }

        // ========================================================
        // DETECT ZOMBIE
        // ========================================================

        if (DetectZombie())
        {
            if (!isTerrified)
            {
                isTerrified = true;

                if (agent != null)
                {
                    agent.isStopped = true;
                }

                // C = 3 → NPC terrified
                SetAnimation(3);

                Debug.Log(
                    "NPC Detected Zombie"
                );
            }

            return;
        }
        else
        {
            if (isTerrified)
            {
                isTerrified = false;

                if (agent != null)
                {
                    agent.isStopped = false;
                }

                Debug.Log(
                    "Zombie Gone"
                );
            }
        }

        // ========================================================
        // FOLLOW OFF
        // ========================================================

        if (!canFollow)
            return;

        // ========================================================
        // CALCULATE DISTANCE
        // ========================================================

        float distance =
            Vector3.Distance(
                transform.position,
                ObjectToFollow.transform.position
            );

        // ========================================================
        // STOP
        // ========================================================

        if (distance < 3f)
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }

            // C = 0 → Idle
            SetAnimation(0);
        }

        // ========================================================
        // NORMAL FOLLOW
        // ========================================================

        else if (
            distance >= 3f &&
            distance < 10f
        )
        {
            if (agent != null)
            {
                agent.isStopped = false;

                agent.speed = 2f;

                agent.SetDestination(
                    ObjectToFollow.transform.position
                );
            }

            // C = 1 → Walk
            SetAnimation(1);
        }

        // ========================================================
        // RUN FOLLOW
        // ========================================================

        else if (distance >= 10f)
        {
            if (agent != null)
            {
                agent.isStopped = false;

                agent.speed = 6f;

                agent.SetDestination(
                    ObjectToFollow.transform.position
                );
            }

            // C = 2 → Run
            SetAnimation(2);
        }
    }

    // ============================================================
    // NPC DEATH
    // ============================================================

    // Được HealthController gọi khi HP <= 0
    public void DieFromHealthController()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log(
            "NPC Died"
        );

        // ========================================================
        // STOP MOVEMENT
        // ========================================================

        if (agent != null)
        {
            agent.isStopped = true;

            if (agent.isOnNavMesh)
            {
                agent.ResetPath();
            }

            agent.enabled = false;
        }

        // ========================================================
        // STOP NPC LOGIC
        // ========================================================

        canFollow = false;

        isTerrified = false;

        // ========================================================
        // DEATH ANIMATION
        // ========================================================

        // C = 4 → Death
        SetAnimation(4);

        // ========================================================
        // BAD END
        // ========================================================

        if (badEndText != null)
        {
            StartCoroutine(
                ShowBadEnd()
            );
        }
    }

    // ============================================================
    // BAD END TEXT
    // ============================================================

    private IEnumerator ShowBadEnd()
    {
        badEndText.SetActive(true);

        Debug.Log(
            "BAD END"
        );

        yield return new WaitForSeconds(2f);

        badEndText.SetActive(false);
    }

    // ============================================================
    // STOP NPC
    // ============================================================

    private void StopNPC()
    {
        if (agent == null)
            return;

        agent.isStopped = true;

        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    // ============================================================
    // ZOMBIE DETECTION
    // ============================================================

    private bool DetectZombie()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                detectZombieRadius
            );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Zombie"))
            {
                return true;
            }

            // Trường hợp collider nằm ở child của Zombie
            if (
                hit.transform.root.CompareTag("Zombie")
            )
            {
                return true;
            }
        }

        return false;
    }

    // ============================================================
    // ANIMATION
    // ============================================================

    private void SetAnimation(int state)
    {
        if (animator == null)
            return;

        animator.SetInteger(
            "C",
            state
        );
    }

    // ============================================================
    // GIZMOS
    // ============================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectZombieRadius
        );
    }
}
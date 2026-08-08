using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCFollow : MonoBehaviour
{
    [Header("Zombie Spawn")]
    public GameObject zombieSpawnObject;
    private bool hasActivatedSpawn = false;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    private bool isDead = false;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Follow")]
    public GameObject ObjectToFollow;
    public bool canFollow = false;

    [Header("Zombie Detection")]
    public float detectZombieRadius = 8f;
    private bool isTerrified = false;

    [Header("Bad End")]
    public GameObject badEndText;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // Khởi tạo máu
        currentHealth = maxHealth;
    }

    void Update()
    {
        // NPC đã chết thì không xử lý logic nữa
        if (isDead)
            return;

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
            // Kích hoạt Zombie Spawn
            if (!hasActivatedSpawn && zombieSpawnObject != null)
            {
                zombieSpawnObject.SetActive(true);
                hasActivatedSpawn = true;
            }
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

    //-----------------------
    // TAKE DAMAGE
    //-----------------------

    public void TakeDamage(float damage)
    {
        // Nếu NPC đã chết thì không nhận damage nữa
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(
            "NPC Take Damage: " + damage +
            " | HP: " + currentHealth
        );


        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Die();
        }
    }

    //-----------------------
    // DIE
    //-----------------------

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("NPC Died");


        // Không cho NPC di chuyển
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // Tắt các trạng thái
        canFollow = false;
        isTerrified = false;

        // Death Animation
        SetAnimation(4);

        // Hiện Bad End
        if (badEndText != null)
        {
            StartCoroutine(ShowBadEnd());
        }
    }

    private IEnumerator ShowBadEnd()
    {
        badEndText.SetActive(true);

        yield return new WaitForSeconds(2f);

        badEndText.SetActive(false);
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

    //-----------------------
    // ANIMATION
    //-----------------------

    void SetAnimation(int state)
    {
        animator.SetInteger("C", state);
    }

    //-----------------------
    // GIZMOS
    //-----------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectZombieRadius);
    }
}
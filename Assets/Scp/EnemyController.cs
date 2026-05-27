using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Transform player;
    [SerializeField] private SimpleObjectPool attackEffectPool;
    [SerializeField] private EnemyAttackTextDisplay attackTextDisplay;

    private CharacterController controller;
    private SimpleHealth targetHealth;
    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private float nextAttackTime;
    private float nextPatrolPickTime;
    private float orbitDirection = 1f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        patrolCenter = transform.position;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            targetHealth = player.GetComponent<SimpleHealth>();
        }

        if (attackTextDisplay == null)
        {
            attackTextDisplay = GetComponentInChildren<EnemyAttackTextDisplay>(true);
        }

        PickPatrolTarget();
    }

    private void Update()
    {
        if (stats == null || player == null)
        {
            Patrol();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stats.DetectRange)
        {
            Patrol();
            return;
        }

        if (distanceToPlayer > stats.AttackRange)
        {
            ChasePlayer();
            return;
        }

        AttackOrEvade();
    }

    private void Patrol()
    {
        if (Time.time >= nextPatrolPickTime || ReachedFlatTarget(patrolTarget, 0.35f))
        {
            PickPatrolTarget();
        }

        MoveTowards(patrolTarget, stats != null ? stats.PatrolSpeed : 2f);
    }

    private void ChasePlayer()
    {
        MoveTowards(player.position, stats.ChaseSpeed);
    }

    private void AttackOrEvade()
    {
        FaceTarget(player.position);

        if (Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + stats.AttackCooldown;
            orbitDirection = Random.value < 0.5f ? -1f : 1f;
            return;
        }

        EvadeAroundPlayer();
    }

    private void AttackPlayer()
    {
        Debug.Log($"{name} attack {player.name} for {stats.AttackDamage} damage.", this);
        if (attackTextDisplay != null)
        {
            attackTextDisplay.ShowAttackText($"Attack -{stats.AttackDamage:0}");
        }

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(stats.AttackDamage);
        }

        if (attackEffectPool != null)
        {
            Vector3 effectPosition = player.position + Vector3.up * 1.1f;
            attackEffectPool.Spawn(effectPosition, Quaternion.identity);
        }
    }

    private void EvadeAroundPlayer()
    {
        Vector3 awayFromPlayer = (transform.position - player.position).normalized;
        Vector3 orbit = Vector3.Cross(Vector3.up, awayFromPlayer) * orbitDirection;
        Vector3 desiredMove = (awayFromPlayer * stats.CooldownBackstepWeight) + orbit;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stats.AttackRange * 0.95f)
        {
            desiredMove += -awayFromPlayer * stats.CooldownBackstepWeight;
        }

        Move(desiredMove.normalized, stats.EvadeSpeed);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Move(direction.normalized, speed);
        FaceTarget(transform.position + direction);
    }

    private void Move(Vector3 direction, float speed)
    {
        Vector3 motion = direction * (speed * Time.deltaTime);
        motion.y = Physics.gravity.y * Time.deltaTime;
        controller.Move(motion);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float turnSpeed = stats != null ? stats.TurnSpeed : 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void PickPatrolTarget()
    {
        float radius = stats != null ? stats.PatrolRadius : 4f;
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        patrolTarget = patrolCenter + new Vector3(randomPoint.x, 0f, randomPoint.y);
        nextPatrolPickTime = Time.time + (stats != null ? stats.PatrolPointLifetime : 3f);
    }

    private bool ReachedFlatTarget(Vector3 target, float distance)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= distance * distance;
    }

    private void OnDrawGizmosSelected()
    {
        if (stats == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.DetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.AttackRange);
    }
}

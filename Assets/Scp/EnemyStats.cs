using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "FPS Project/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Ranges")]
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float attackRange = 2.5f;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 1.8f;
    [SerializeField] private float chaseSpeed = 3.4f;
    [SerializeField] private float evadeSpeed = 2.6f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float patrolRadius = 4f;
    [SerializeField] private float patrolPointLifetime = 3f;
    [SerializeField] private float cooldownBackstepWeight = 0.7f;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;

    public float DetectRange => detectRange;
    public float AttackRange => attackRange;
    public float PatrolSpeed => patrolSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float EvadeSpeed => evadeSpeed;
    public float TurnSpeed => turnSpeed;
    public float PatrolRadius => patrolRadius;
    public float PatrolPointLifetime => patrolPointLifetime;
    public float CooldownBackstepWeight => cooldownBackstepWeight;
    public float AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
}

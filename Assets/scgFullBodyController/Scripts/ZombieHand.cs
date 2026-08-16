using UnityEngine;
using scgFullBodyController;

public class ZombieHand : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 20;

    [SerializeField]
    private float damageCooldown = 1f;

    private Animator animator;

    private float nextDamageTime;

    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        animator =
            GetComponentInParent<Animator>();
    }

    // ============================================================
    // TRIGGER
    // ============================================================

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(
            "ZombieHand OnTriggerEnter: " +
            other.name
        );

        TryDamageTarget(
            other.transform
        );
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamageTarget(
            other.transform
        );
    }

    // ============================================================
    // COLLISION
    // ============================================================

    private void OnCollisionEnter(
        Collision collision
    )
    {
        Debug.Log(
            "ZombieHand OnCollisionEnter: " +
            collision.gameObject.name
        );

        TryDamageTarget(
            collision.transform
        );
    }

    private void OnCollisionStay(
        Collision collision
    )
    {
        TryDamageTarget(
            collision.transform
        );
    }

    // ============================================================
    // TRY DAMAGE
    // ============================================================

    private void TryDamageTarget(
        Transform hitTransform
    )
    {
        if (hitTransform == null)
            return;

        // ========================================================
        // CHECK ATTACK COOLDOWN
        // ========================================================

        if (!CanDamage())
            return;

        // ========================================================
        // KHÔNG ĐÁNH CHÍNH ZOMBIE
        // ========================================================

        Transform hitRoot =
            hitTransform.root;

        if (
            hitRoot.CompareTag("Zombie")
        )
        {
            return;
        }

        // ========================================================
        // FIND HEALTH CONTROLLER
        // ========================================================

        HealthController healthController =
            hitTransform.GetComponentInParent<HealthController>();

        if (healthController == null)
        {
            Debug.Log(
                "ZombieHand: No HealthController found on " +
                hitTransform.name
            );

            return;
        }

        // ========================================================
        // ALREADY DEAD
        // ========================================================

        if (healthController.IsDead())
        {
            return;
        }

        // ========================================================
        // CHECK PLAYER / NPC
        // ========================================================

        bool isPlayer =
            hitRoot.CompareTag("Player");

        bool isNPC =
            hitRoot.CompareTag("NPC");

        if (!isPlayer && !isNPC)
        {
            return;
        }

        // ========================================================
        // DAMAGE
        // ========================================================

        Debug.Log(
            "Zombie attacks: " +
            hitRoot.name +
            " | Damage: " +
            damage
        );

        healthController.Damage(
            damage
        );

        // ========================================================
        // COOLDOWN
        // ========================================================

        nextDamageTime =
            Time.time + damageCooldown;
    }

    // ============================================================
    // CAN DAMAGE
    // ============================================================

    private bool CanDamage()
    {
        if (Time.time < nextDamageTime)
        {
            return false;
        }

        // Nếu không lấy được Animator
        // thì vẫn cho phép damage
        if (animator == null)
        {
            return true;
        }

        // Chỉ gây damage khi Zombie đang attack
        return animator.GetBool(
            "isAttacking"
        );
    }
}
using UnityEngine;
using scgFullBodyController;

public class ZombieHand : MonoBehaviour
{
    public int damage = 20;

    [SerializeField]
    private float damageCooldown = 1f;

    private Animator animator;
    private float nextDamageTime;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ZombieHand OnTriggerEnter: " + other.name);
        TryDamageTarget(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("ZombieHand OnTriggerStay: " + other.name);
        TryDamageTarget(other.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ZombieHand OnCollisionEnter: " + collision.gameObject.name);
        TryDamageTarget(collision.transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("ZombieHand OnCollisionStay: " + collision.gameObject.name);
        TryDamageTarget(collision.transform);
    }

    private void TryDamageTarget(Transform hitTransform)
    {
        if (!CanDamage())
        {
            return;
        }

        Transform targetRoot = hitTransform.root;

        if (!hitTransform.CompareTag("Player") &&
            !hitTransform.CompareTag("NPC") &&
            !targetRoot.CompareTag("Player") &&
            !targetRoot.CompareTag("NPC"))
        {
            return;
        }
        Debug.Log(
        "ZombieHand HIT: " + hitTransform.name +
        " | Root: " + targetRoot.name +
        " | Tag: " + hitTransform.tag +
        " | Root Tag: " + targetRoot.tag
        );

        // =========================
        // NPC
        // =========================

        if (targetRoot.CompareTag("NPC"))
        {
            NPCFollow npc = targetRoot.GetComponent<NPCFollow>();

            if (npc != null)
            {
                npc.TakeDamage(damage);
                nextDamageTime = Time.time + damageCooldown;
            }

             return;
        }

        // =========================
        // PLAYER
        // =========================

        if (targetRoot.CompareTag("Player"))
        {
        HealthController healthController = targetRoot.GetComponent<HealthController>();

        if (healthController == null)
        {
            return;
        }

        healthController.Damage(damage);
        nextDamageTime = Time.time + damageCooldown;
        }
    }

    private bool CanDamage()
    {
        //Debug.Log("isAttacking = " + animator.GetBool("isAttacking"));
        if (Time.time < nextDamageTime)
        {
            return false;
        }

        return animator == null || animator.GetBool("isAttacking");
    }
}

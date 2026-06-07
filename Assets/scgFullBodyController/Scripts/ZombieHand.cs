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
        TryDamagePlayer(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamagePlayer(other.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryDamagePlayer(collision.transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        TryDamagePlayer(collision.transform);
    }

    private void TryDamagePlayer(Transform hitTransform)
    {
        if (!CanDamage())
        {
            return;
        }

        if (!hitTransform.CompareTag("Player"))
        {
            return;
        }

        HealthController healthController = hitTransform.root.GetComponent<HealthController>();

        if (healthController == null)
        {
            return;
        }

        healthController.Damage(damage);
        nextDamageTime = Time.time + damageCooldown;
    }

    private bool CanDamage()
    {
        if (Time.time < nextDamageTime)
        {
            return false;
        }

        return animator == null || animator.GetBool("isAttacking");
    }
}

using UnityEngine;

public class SimpleHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
        {
            Debug.Log($"{name} is already dead. Cannot take more damage.", this);
            return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);
        Debug.Log($"{name} health: {CurrentHealth}", this);
    }
}

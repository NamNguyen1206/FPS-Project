using UnityEngine;
using scgFullBodyController;

public class ArmorPickup : MonoBehaviour
{
    [SerializeField] private float armorAmount = 100f;
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        HealthController health = other.GetComponent<HealthController>();
        if (health != null)
        {
            health.AddArmor(armorAmount);
            collected = true;
            gameObject.SetActive(false);
        }
    }
}

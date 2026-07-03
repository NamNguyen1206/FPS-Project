using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GrenadeInventory inventory =
            other.GetComponent<GrenadeInventory>();

        if (inventory != null)
        {
            inventory.AddGrenade(amount);

            Destroy(gameObject);
        }
    }
}

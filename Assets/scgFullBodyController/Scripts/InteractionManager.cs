using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace scgFullBodyController
{
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public WeaponPickup hoveredWeapon;

    [SerializeField]
    private WeaponInventory inventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        DetectWeapon();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupWeapon();
        }
    }

    private void DetectWeapon()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            WeaponPickup pickup =
                hit.transform.GetComponent<WeaponPickup>();

            if (pickup != null)
            {
                hoveredWeapon = pickup;
                return;
            }
        }

        hoveredWeapon = null;
    }

    private void PickupWeapon()
    {
        if (hoveredWeapon == null)
            return;

        inventory.AddWeapon(hoveredWeapon.weaponType);

        Destroy(hoveredWeapon.gameObject);
    }
}
}
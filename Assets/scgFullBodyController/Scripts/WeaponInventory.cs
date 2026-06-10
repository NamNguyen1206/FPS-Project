using UnityEngine;
namespace scgFullBodyController
{
public class WeaponInventory : MonoBehaviour
{
    [Header("Owned Weapons")]
    public bool hasPistol = true;
    public bool hasRifle = false;
    public bool hasSniper = false;

    public void AddWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                hasPistol = true;
                break;

            case WeaponType.Rifle:
                hasRifle = true;
                break;

            case WeaponType.Sniper:
                hasSniper = true;
                break;
        }

        Debug.Log("Picked up: " + weaponType);
    }
}
}

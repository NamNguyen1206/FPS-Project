// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class WeaponUI : MonoBehaviour
// {
//     [Header("Armor")]
//     public TextMeshProUGUI magazineAmmoUI;
//     public TextMeshProUGUI totalAmmoUI;
//     [Header("Weapon")]
//     public Image activeWeaponUI;
//     [Header("Throwables")]
//     public Image lethalUI;
//     public TextMeshProUGUI lethalAountUI;

//     private void Update()
//     {
//         Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponent<Weapon>();

//         if(activeWeapon )
//         {
//             magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBust}";
//             totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBust}";
//             Weapon.WeaponType weaponType = activeWeapon.weaponType;

//             activeWeaponUI.sprite = GetWeaponSprite(weaponType);
//         }
//         else
//         {
//             magazineAmmoUI.text = "";
//             totalAmmoUI.text = "";
//         }
//     }
//     private Sprite GetWeaponSprite(Weapon.WeaponType weaponType)
//     {
//         switch (weaponType)
//         {
//             case Weapon.WeaponType.Pistol:
//                 return Instantiate(Resources.Load<GameObject>("Pistol")).GetComponent<SpriteRenderer>().sprite;
//             case Weapon.WeaponType.Rifle:
//                 return Instantiate(Resources.Load<GameObject>("Rifle")).GetComponent<SpriteRenderer>().sprite;
//             case Weapon.WeaponType.Sniper:
//                 return Instantiate(Resources.Load<GameObject>("Sniper")).GetComponent<SpriteRenderer>().sprite;
//             default:
//                 return null;
//         }
//     }

// }




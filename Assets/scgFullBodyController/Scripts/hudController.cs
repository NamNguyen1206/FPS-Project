using UnityEngine;
using UnityEngine.UI;

namespace scgFullBodyController
{
    public class hudController : MonoBehaviour
    {
        //Simple references to the HUD for other scripts to access and modify
        public Text uiHealth;
        public Text uiBullets;
        public Text uiArmor;
        public GameObject crosshair;

        [Header("Health Bar")]
        public float maxHealth;
        public Image frontHealthBar;
        public Image backHealthBar;
        public float chipSpeed = 2f;
        public float healSpeedMultiplier = 2f;

        [Header("Armor Bar")]
        public Image armorBar;
        public float maxArmor = 100f;

        private float health;
        private float lerpTime;

        void Start()
        {
            health = maxHealth;
            SetHealth(health, maxHealth);
        }

        void Update()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();
        }

        public void SetHealth(float currentHealth, float currentMaxHealth)
        {
            maxHealth = Mathf.Max(currentMaxHealth, 1f);
            health = Mathf.Clamp(currentHealth, 0f, maxHealth);
            lerpTime = 0f;

            if (uiHealth != null)
            {
                uiHealth.text = health.ToString("0");
            }
        }

        public void SetArmor(float armor)
        {
            armor = Mathf.Clamp(armor, 0f, maxArmor);

            if (uiArmor != null)
            {
                uiArmor.text = armor.ToString("0");
            }

            if (armorBar != null)
            {
                armorBar.fillAmount = armor / Mathf.Max(maxArmor, 1f);
            }
        }

        public void UpdateHealthUI()
        {
            if (frontHealthBar == null || backHealthBar == null)
            {
                return;
            }

            float fillF = frontHealthBar.fillAmount;
            float fillB = backHealthBar.fillAmount;
            float hFraction = health / maxHealth;

            if (fillB > hFraction)
            {
                frontHealthBar.fillAmount = hFraction;
                backHealthBar.color = Color.red;
                lerpTime += Time.deltaTime;
                float percentComplete = lerpTime / chipSpeed; 
                percentComplete *= percentComplete;
                backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            }
            if (fillF < hFraction)
            {
                backHealthBar.color = Color.green;
                backHealthBar.fillAmount = hFraction;
                lerpTime += Time.deltaTime;
                float percentComplete = (lerpTime * healSpeedMultiplier) / chipSpeed;
                percentComplete *= percentComplete;
                frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            }
        }
        public void RestoreHealth(float healthToRestore)
        {
            health += healthToRestore;
            health = Mathf.Clamp(health, 0, maxHealth);
            lerpTime = 0f;
        }
    }
}

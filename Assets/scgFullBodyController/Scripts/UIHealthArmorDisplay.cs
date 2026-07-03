using UnityEngine;
using scgFullBodyController;

public class UIHealthArmorDisplay : MonoBehaviour
{
    private HealthController healthController;
    private hudController hudController;

    private void Start()
    {
        healthController = GetComponent<HealthController>();

        GameObject hudUI = GameObject.FindGameObjectWithTag("hud");
        if (hudUI != null)
        {
            hudController = hudUI.GetComponent<hudController>();
        }
    }

    private void Update()
    {
        if (hudController == null || healthController == null)
            return;

        // if (healthController.health > 0)
        // {
        //     hudController.uiHealth.text = healthController.health.ToString();
        // }
        // else
        // {
        //     hudController.uiHealth.text = "0";
        // }

        // hudController.uiArmor.text = healthController.armor.ToString();
    }
}

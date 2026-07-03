using UnityEngine;
using scgFullBodyController;

public class GrenadeInventory : MonoBehaviour
{
    public int grenadeCount = 0;
    public int maxGrenades = 5;

    public int CurrentGrenades
    {
        get { return grenadeCount; }
    }
    public bool HasGrenade()
    {
        return grenadeCount > 0;
    }

    public void AddGrenade(int amount)  
    {
        grenadeCount = Mathf.Min(grenadeCount + amount, maxGrenades);

        Debug.Log("Grenades: " + grenadeCount);

        hudController hud = GameObject.FindGameObjectWithTag("hud")
                                      .GetComponent<hudController>();

        if (hud != null)
        {
            hud.SetGrenades(grenadeCount);
        }
    }
    
    public bool UseGrenade()
    {
        if (grenadeCount <= 0)
            return false;

        grenadeCount--;

        hudController hud = GameObject.FindGameObjectWithTag("hud")
                                      .GetComponent<hudController>();

        if (hud != null)
        {
            hud.SetGrenades(grenadeCount);
        }

        return true;
    }
}

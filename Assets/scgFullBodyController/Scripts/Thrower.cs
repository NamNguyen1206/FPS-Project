using UnityEngine;
using UnityEngine.UIElements;

public class Thrower : MonoBehaviour
{
    [Header("Grenade Inventory")]
    [SerializeField] private GameObject grenadePrefab;
    private GrenadeInventory inventory;

    [Header("Grenade Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.G;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0,1,0);

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxForce = 20f;

    [Header("Trajectory Setting")]
    [SerializeField] private LineRenderer trajectoryLine;


    private bool isCharging = false;
    private float chargeTime = 0f;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (inventory == null)
        // {
        //     Debug.LogError("GrenadeInventory not found!");
        // }
        inventory = GetComponent<GrenadeInventory>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(throwKey))
        {
            StartThrowing();
        }
        if(isCharging)
        {
            ChargeThrow();
        }
        if(Input.GetKeyUp(throwKey))
        {
            ReleaseThrow();
        }
    }
    void StartThrowing()
    {
        if (inventory == null || !inventory.HasGrenade())
        {
            Debug.Log("No Grenades!");
            return;
        }
        isCharging = true;
        chargeTime = 0f;
        trajectoryLine.enabled = true;
    }
    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;
        Vector3 grenadeVelocity = (mainCamera.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * throwForce, maxForce);
        ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);
    }
    void ReleaseThrow()
    {   if (!isCharging)
        return;

        if (inventory.UseGrenade())
        {
            ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        }
        isCharging = false;
        trajectoryLine.enabled = false;
    }
    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position +  mainCamera.transform.forward;
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, mainCamera.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        Vector3 finalThrowDirection = (mainCamera.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
    }
    void ShowTrajectory (Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3 [100];
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }
        trajectoryLine.SetPositions(points);
    }
}

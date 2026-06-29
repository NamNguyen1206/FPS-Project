using UnityEngine;

public class Thrower : MonoBehaviour
{
    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab;
    [Header("Grenade Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0,1,0);
    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxForce = 20f;


    private bool isCharging = false;
    private float chargeTime = 0f;
    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        isCharging = true;
        chargeTime = 0f;

    }
    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;
    }
    void ReleaseThrow()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        isCharging = false;
    }
    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position +  mainCamera.transform.forward;
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, mainCamera.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        Vector3 finalThrowDirection = (mainCamera.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingGrenate : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject exoplosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticalOffset = new Vector3(0,1,0);
    
    [Header("Explosion Setting")]
    [SerializeField] private float explosionDeley = 3f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;

    private float Countdown;
    private bool hasExploded = false; 
    private void Start()
    {
        Countdown = explosionDeley;
    }
    private void Update()
    {
        if(!hasExploded)
        {
            Countdown -= Time.deltaTime;
            if(Countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }
    void Explode()
    {
        GameObject explosionEffect = Instantiate(exoplosionEffectPrefab, transform.position + explosionParticalOffset, Quaternion.identity);
        Destroy(explosionEffect,4f);
        Destroy(gameObject);
    }
    void NearbyForceApply()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,explosionRadius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position,explosionRadius);
            }
        }
    }
}

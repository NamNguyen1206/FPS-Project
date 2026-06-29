using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenate : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject exoplosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticalOffset = new Vector3(0,1,0);
    [SerializeField] private GameObject audioSourcePrefab;
    
    [Header("Explosion Setting")]
    [SerializeField] private float explosionDeley = 3f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip impactSound;
    private float Countdown;
    private bool hasExploded = false; 
    private AudioSource audioSource;

    private void Start()
    {
        Countdown = explosionDeley;
        audioSource = GetComponent<AudioSource>();
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
        PlaySoundAtPosition(explosionSound);
        NearbyForceApply();
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
    void PlaySoundAtPosition(AudioClip clip)
    {
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource instantiateAudioSource = audioSourceObject.GetComponent<AudioSource>();
        instantiateAudioSource.clip = clip;
        instantiateAudioSource.spatialBlend = 1;
        instantiateAudioSource.Play();

        Destroy(audioSourceObject, instantiateAudioSource.clip.length);
    }
    private void OnCollisionEnter(Collision collision)
    {
        audioSource.clip = impactSound;
        audioSource.spatialBlend = 1;
        audioSource.Play();
    }
}

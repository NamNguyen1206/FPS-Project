using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieFootsteps : MonoBehaviour
{
    [Header("Footstep Sounds")]
    public AudioClip[] grassSounds;
    public AudioClip[] concreteSounds;
    public AudioClip[] metalSounds;
    public AudioClip[] gravelSounds;
    public AudioClip[] waterSounds;

    [Header("Settings")]
    public float moveThreshold = 0.2f;// Minimum speed to trigger footsteps
    public float stepInterval = 0.45f;

    private AudioSource audioSource;
    private NavMeshAgent agent;
    private string floorTag = "";

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //agent = GetComponent<NavMeshAgent>();
    }

    // private void Start()
    // {
    //     StartCoroutine(FootstepRoutine());
    // }

    // private IEnumerator FootstepRoutine()
    // {
    //     while (true)
    //     {
    //         //Debug.Log("Zombie Speed = " + agent.velocity.magnitude);
    //         if (agent.enabled &&
    //             !agent.isStopped &&
    //             agent.velocity.magnitude > moveThreshold)
    //         {
    //             PlayFootstep();

    //             // float speedPercent = agent.velocity.magnitude / agent.speed;
    //             // float interval = Mathf.Lerp(0.6f, 0.25f, speedPercent);
    //             // yield return new WaitForSeconds(interval);

    //             yield return new WaitForSeconds(stepInterval);
    //         }
    //         else
    //         {
    //             yield return null;
    //         }
    //     }
    // }

    private void PlayFootstep()
    {
        //Debug.Log("Play Footstep");
        //Debug.Log("Floor = " + floorTag);
        floorTag = GetFloorTag();
        AudioClip clip = null;

        switch (floorTag)
        {
            case "grass":
                if (grassSounds.Length > 0)
                    clip = grassSounds[Random.Range(0, grassSounds.Length)];
                break;

            case "concrete":
                if (concreteSounds.Length > 0)
                    clip = concreteSounds[Random.Range(0, concreteSounds.Length)];
                break;

            case "metal":
                if (metalSounds.Length > 0)
                    clip = metalSounds[Random.Range(0, metalSounds.Length)];
                break;

            case "gravel":
                if (gravelSounds.Length > 0)
                    clip = gravelSounds[Random.Range(0, gravelSounds.Length)];
                break;

            case "water":
                if (waterSounds.Length > 0)
                    clip = waterSounds[Random.Range(0, waterSounds.Length)];
                break;
        }

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // private void OnCollisionEnter(Collision col)
    // {
    //     floorTag = col.collider.tag;
    //     Debug.Log("Floor = " + col.collider.tag);
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     floorTag = other.tag;
    // }

    private string GetFloorTag()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up,
                            Vector3.down,
                            out hit,
                            3f))
        {
            return hit.collider.tag;
        }

        return "";
    }
}
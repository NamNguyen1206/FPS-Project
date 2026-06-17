using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ZombiepatrolingState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime = 10f;

    private Transform player;
    private NavMeshAgent navAgent;

    public float detectionArea = 18f;
    public float patrolspeed = 2f;

    private List<Transform> waypointList = new List<Transform>();
    private int currentWaypointIndex;

    // Called when entering Patrol State
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ENTER PATROL STATE");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        navAgent = animator.GetComponent<NavMeshAgent>();

        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent not found on Zombie!");
            return;
        }

        navAgent.speed = patrolspeed;
        timer = 0f;

        waypointList.Clear();

        ZombieController zombie = animator.GetComponent<ZombieController>();

        if (zombie != null)
        {
            waypointList.AddRange(zombie.patrolPoints);
        }

        currentWaypointIndex = 0;

        if (waypointList.Count > 0)
        {
            navAgent.SetDestination(
                waypointList[currentWaypointIndex].position
            );

            Debug.Log("Going To: " +
                      waypointList[currentWaypointIndex].name);
        }
        else
        {
            Debug.LogWarning(
                animator.name +
                " has no patrol points assigned!"
            );
        }
    }

    // Called every frame while in Patrol State
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
            return;

        // Move to next waypoint when reaching current one
        if (waypointList.Count > 0 &&
            !navAgent.pathPending &&
            navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypointList.Count)
            {
                currentWaypointIndex = 0;
            }

            navAgent.SetDestination(
                waypointList[currentWaypointIndex].position
            );
        }

        timer += Time.deltaTime;

        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        // Detect Player
        if (player != null)
        {
            float distanceToPlayer =
                Vector3.Distance(
                    player.position,
                    animator.transform.position
                );

            if (distanceToPlayer < detectionArea)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    // Called when leaving Patrol State
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent != null &&
            navAgent.enabled &&
            navAgent.isOnNavMesh)
        {
            navAgent.ResetPath();
        }
    }
}
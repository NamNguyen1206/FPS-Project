using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ZombiepatrolingState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime = 30f;

    private ZombieTarget zombieTarget;
    private NavMeshAgent navAgent;

    public float detectionArea = 18f;
    public float patrolspeed = 0.5f;

    private List<Transform> waypointList = new List<Transform>();
    private int currentWaypointIndex;

    // Called when entering Patrol State
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("ENTER PATROL STATE");

        zombieTarget = animator.GetComponent<ZombieTarget>();
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

            //Debug.Log("Going To: " + waypointList[currentWaypointIndex].name);
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

        Transform target = zombieTarget != null ? zombieTarget.currentTarget : null;

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.position, animator.transform.position);

            if (distanceToTarget < detectionArea)
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

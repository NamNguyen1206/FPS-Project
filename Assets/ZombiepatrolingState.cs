using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class ZombiepatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 0f;
    Transform player;
    NavMeshAgent navAgent;

    public float detectionArea = 18f;
    public float patrolspeed = 2f;

    List<Transform> waypointList = new List<Transform>();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       navAgent = animator.GetComponent<NavMeshAgent>();

       navAgent.speed = patrolspeed;
       timer = 0;

       // -- Get all waypoints and move to first waypoint --//
       waypointList.Clear(); // Clear previous waypoints to avoid duplicates
       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
       if(waypointCluster != null)
       {
           foreach(Transform waypoint in waypointCluster.transform)
           {
               waypointList.Add(waypoint);
           }
       }
       
       if(waypointList.Count > 0)
       {
           Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
           navAgent.SetDestination(nextPosition);
       }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Check if we reached the destination and move to next waypoint
        if(waypointList.Count > 0 && navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            navAgent.SetDestination(waypointList[Random.Range(0, waypointList.Count)].position);
        }

        timer += Time.deltaTime;
        if(timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceToPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       navAgent.SetDestination(animator.transform.position);
    }
}

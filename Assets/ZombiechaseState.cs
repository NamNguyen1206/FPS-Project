using UnityEngine;
using UnityEngine.AI;

public class ZombiechaseState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    Transform player;
    public float chaseSpeed = 6f;
    public float stopChasingDistance = 20f;
    public float attackingDistance = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent.speed = chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       navAgent.SetDestination(player.position);
       animator.transform.LookAt(player);

       float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);

        if(distanceToPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        if(distanceToPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       navAgent.SetDestination(animator.transform.position);
    }
}

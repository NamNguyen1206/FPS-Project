using UnityEngine;
using UnityEngine.AI;

public class ZombieattackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float stopAttackingDistance = 3f;

// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;
       player = GameObject.FindGameObjectWithTag("Player").transform;
       navAgent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //     if (player == null)
    // {
    //     animator.SetBool("isAttacking", false);
    //     return;
    // } 
        if (layerIndex != 0 || player == null || navAgent == null) return;
       LookAtPlayer();

       float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);

        if(distanceToPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }



    private void LookAtPlayer()
    {
        if (player == null || navAgent == null) return;
        Vector3 direction = player.position - navAgent.transform.position;
        navAgent.transform.rotation = Quaternion.LookRotation(direction);
        var yRotation = navAgent.transform.eulerAngles.y;
        navAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    // if (player == null || navAgent == null)
    //     return;

    // Vector3 direction =
    // player.position - navAgent.transform.position;
    // direction.y = 0f;

    // if (direction.sqrMagnitude < 0.01f)
    //     return;

    // navAgent.transform.rotation = Quaternion.LookRotation(direction);
    }
}

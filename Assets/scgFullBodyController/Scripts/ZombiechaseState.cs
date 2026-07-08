using UnityEngine;
using UnityEngine.AI;

public class ZombiechaseState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    ZombieTarget zombieTarget;
    public float chaseSpeed = 3f;
    public float stopChasingDistance = 20f;
    public float attackingDistance = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;
        navAgent = animator.GetComponent<NavMeshAgent>();
        zombieTarget = animator.GetComponent<ZombieTarget>();

        if (navAgent != null)
        {
            navAgent.speed = chaseSpeed;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0 || navAgent == null) return;

        Transform target = zombieTarget != null ? zombieTarget.currentTarget : null;

        if (target == null)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
            return;
        }

        navAgent.SetDestination(target.position);
        LookAtTarget(animator.transform, target);

        float distanceToTarget = Vector3.Distance(target.position, animator.transform.position);

        if(distanceToTarget > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }

        if(distanceToTarget < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;

        if (navAgent != null && navAgent.isOnNavMesh)
        {
       navAgent.SetDestination(animator.transform.position);
        }
    }
    
    private void LookAtTarget(Transform zombieTransform, Transform target)
    {
        Vector3 direction = target.position - zombieTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        zombieTransform.rotation = Quaternion.LookRotation(direction);
    }
}

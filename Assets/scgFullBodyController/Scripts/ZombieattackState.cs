using UnityEngine;
using UnityEngine.AI;

public class ZombieattackState : StateMachineBehaviour
{
    ZombieTarget zombieTarget;
    NavMeshAgent navAgent;

    public float stopAttackingDistance = 3f;

// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;
        zombieTarget = animator.GetComponent<ZombieTarget>();
        navAgent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0 || navAgent == null) return;

        Transform target = zombieTarget != null ? zombieTarget.currentTarget : null;

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", false);
            return;
        }

        LookAtTarget(target);

        float distanceToTarget = Vector3.Distance(target.position, animator.transform.position);

        if(distanceToTarget > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
        }
    }

    private void LookAtTarget(Transform target)
    {
        if (target == null || navAgent == null) return;
        Vector3 direction = target.position - navAgent.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        navAgent.transform.rotation = Quaternion.LookRotation(direction);
    }
}

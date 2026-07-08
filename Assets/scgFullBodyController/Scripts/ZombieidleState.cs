using UnityEngine;

public class ZombieidleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0.1f;
    ZombieTarget zombieTarget;

    public float detectionAreaEnter = 18f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;
        timer = 0;
        zombieTarget = animator.GetComponent<ZombieTarget>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex != 0) return;
       timer += Time.deltaTime;
       if(timer > idleTime)
       {
           animator.SetBool("isPatroling", true);
       }

        Transform target = zombieTarget != null ? zombieTarget.currentTarget : null;

        if (target != null && Vector3.Distance(target.position, animator.transform.position) < detectionAreaEnter)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (layerIndex != 0) return;
    }

}

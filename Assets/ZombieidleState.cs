using UnityEngine;

public class ZombieidleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;
    Transform player;

    public float detectionAreaEnter = 18f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       timer = 0;
       player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       timer += Time.deltaTime;
       if(timer > idleTime)
       {
           animator.SetBool("isPatroling", true);
       }

       float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceToPlayer < detectionAreaEnter)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}

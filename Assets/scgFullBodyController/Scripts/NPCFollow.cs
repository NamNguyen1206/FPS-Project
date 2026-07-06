using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    public GameObject ObjectToFollow;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, ObjectToFollow.transform.position);
        if(distance < 3)
        {
            agent.isStopped = true;
            animator.SetInteger("C",0);
        }
        else if (distance >= 3 && distance < 10)
        {
            //Ai walking
            agent.isStopped = false;
            agent.SetDestination(ObjectToFollow.transform.position);
            animator.SetInteger("C",1);
            agent.speed = 2;
        }
        else if(distance > 10)
        {
            //Ai Running
            agent.isStopped = false ;
            agent.SetDestination(ObjectToFollow.transform.position);
            animator.SetInteger("C",2);
            agent.speed = 6;
        }
    }
}

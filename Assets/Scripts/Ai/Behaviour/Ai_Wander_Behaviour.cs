using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Ai_Wander_Behaviour : StateMachineBehaviour
{

    NavMeshAgent agent;
    AiController aiController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiController = animator.gameObject.GetComponent<AiController>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();


        NavMeshHit hit;
        NavMesh.SamplePosition(Random.insideUnitSphere * aiController.RandomWanderDistance, out hit, aiController.RandomWanderDistance, 1);
        agent.destination = hit.position;

        //agent.sto
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((agent.destination - agent.transform.position).magnitude <= agent.stoppingDistance + 1)
        {
            animator.SetBool("CanWander", false);
        }
        else
        {
            // Debug.Log((agent.destination - agent.transform.position).magnitude + " : " + agent.stoppingDistance + 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

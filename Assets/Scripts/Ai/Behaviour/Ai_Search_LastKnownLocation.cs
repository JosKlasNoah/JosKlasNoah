using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_Search_LastKnownLocation : StateMachineBehaviour
{

    NavMeshAgent agent;
    AiController aiConctroller;

    bool doOnce;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiConctroller = animator.gameObject.GetComponent<AiController>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();

        agent.destination = aiConctroller.LastTargetLocation;
        doOnce = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((agent.destination - agent.transform.position).magnitude <= agent.stoppingDistance + 1)
        {
            if (!doOnce)
            {
                animator.SetTrigger("OnLastKnowPosistion");
                animator.SetBool("Target", false);
                animator.SetBool("HasLastTargetLocation", false);

                doOnce = true;
            }
        }
        else
        {
            // Debug.Log((agent.destination - agent.transform.position).magnitude + " : " + agent.stoppingDistance + 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

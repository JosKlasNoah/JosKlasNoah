using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Search_LookAround_Behaviour : StateMachineBehaviour
{
    AiController aiConctroller;

    float lookTime;

    [SerializeField]
    Quaternion oldRotation;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiConctroller = animator.GetComponent<AiController>();
        oldRotation = aiConctroller.transform.rotation;
        lookTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiConctroller.transform.rotation = Quaternion.Euler(0, oldRotation.y +( Mathf.Sin(lookTime * aiConctroller.LookRotationSpeed) * (aiConctroller.LookRotation / 2)), 0);

        lookTime += Time.deltaTime;

        if (lookTime * aiConctroller.LookRotationSpeed > Mathf.PI * 2)
            animator.SetTrigger("FinishedLookingAround");
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

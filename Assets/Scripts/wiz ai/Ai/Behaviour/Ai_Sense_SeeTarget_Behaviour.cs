using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ai_Sense_SeeTarget_Behaviour : StateMachineBehaviour
{
    AiController aiConctroller;
    Transform aiTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiConctroller = animator.gameObject.GetComponent<AiController>();
        aiTransform = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //debug rays
        Debug.DrawRay(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, aiTransform.forward * aiConctroller.LookDepth, Color.red);
        Debug.DrawRay(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, (aiTransform.forward - aiTransform.right * aiConctroller.EyeSight).normalized * aiConctroller.LookDepth, Color.red);
        Debug.DrawRay(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, (aiTransform.forward + aiTransform.right * aiConctroller.EyeSight).normalized * aiConctroller.LookDepth, Color.red);

        

        Ray[] rayList = new Ray[]
        {
            new Ray(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, aiTransform.forward * aiConctroller.LookDepth),
            new Ray(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, (aiTransform.forward - aiTransform.right * aiConctroller.EyeSight).normalized * aiConctroller.LookDepth),
            new Ray(aiTransform.position + aiTransform.up * aiConctroller.EyeHeight, (aiTransform.forward + aiTransform.right * aiConctroller.EyeSight).normalized * aiConctroller.LookDepth)
        };

        foreach (Ray item in rayList)
        {
            RaycastHit hit;

            if ( Physics.Raycast(item, out hit) )
            {
                //Should create a better solution for this

                PlayerController FoundItem = hit.collider.gameObject.GetComponent<PlayerController>();

                if ( FoundItem != null ) {
                    aiConctroller.Target = FoundItem;
                    Debug.Log("found player");
                }
                else
                    Debug.Log(hit.collider.gameObject);
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ai_Combat_AttackBasic : StateMachineBehaviour
{
    [SerializeField, Range( 0.5f, 4 )]
    private float _cooldown = 1.5f;

    private IEnumerator attack;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        attack = Attack( animator );
        animator.gameObject.GetComponent<AiController>().StartCoroutine( attack );
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<AiController>().StopCoroutine( attack );
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    IEnumerator Attack( Animator pAnimator ) {
        while ( true ) {
            pAnimator.gameObject.GetComponent<WizardAI>().InstantiateAttack();

            yield return new WaitForSeconds( _cooldown );
        }
    }
}

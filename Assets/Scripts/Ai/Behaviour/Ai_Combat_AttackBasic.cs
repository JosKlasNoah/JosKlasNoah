using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Ai_Combat_AttackBasic : StateMachineBehaviour
{
    [SerializeField, Range( 0.5f, 4 )]
    private float _cooldown = 1.5f;

    private AiController _aiController;
    private Transform _aiTransform;

    private int _damping = 2;
    private float t = 0;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex ) {
        _aiController = animator.gameObject.GetComponent<AiController>();
        _aiTransform = animator.transform;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        Vector3 targetPosition = _aiController.Target.transform.position;
        FaceTarget( targetPosition );
        CheckRange( animator, targetPosition );
        AttackUsing( animator.GetComponent<WizardAI>() );
    }

    private void FaceTarget( Vector3 pTargetPosition )
    {
        Vector3 lookPos = pTargetPosition - _aiTransform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation( lookPos );
        _aiTransform.rotation = Quaternion.Slerp( _aiTransform.rotation, rotation, Time.deltaTime * _damping );
    }

    private void CheckRange( Animator pAnimator, Vector3 targetPosition )
    {
        if ( _aiController.Target != null && Vector3.Distance( _aiTransform.position, _aiController.Target.transform.position ) > _aiController.AttackRange )
        {
            pAnimator.SetBool( "InAttackRange", false );
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void AttackUsing( WizardAI wizardAI ) {
        t -= Time.deltaTime;
        if ( t <= 0 ) {
            t = _cooldown;

            wizardAI.InstantiateAttack(_aiController.Target.transform);
        }
    }
}

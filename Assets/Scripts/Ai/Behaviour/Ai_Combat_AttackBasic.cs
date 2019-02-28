using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Ai_Combat_AttackBasic : StateMachineBehaviour
{
    private float _cooldown;

    private AiController _aiController;
    private Transform _aiTransform;

    private float _damping = 3;
    private float _t = 0;
    private bool _inAttackRange = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex ) {
        _aiController = animator.gameObject.GetComponent<AiController>();
        _aiTransform = animator.transform;
        _cooldown = _aiController.Cooldown;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        if ( null != _aiController.Target ) {
            Vector3 targetPosition = _aiController.Target.transform.position;
            FaceTarget( targetPosition );
            CheckRange( animator, targetPosition );
            AttackUsing( animator.GetComponent<WizardAI>() );
        }
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
        if ( _aiController.Target != null )
        {
            _inAttackRange = Vector3.Distance( _aiTransform.position, _aiController.Target.transform.position ) <= _aiController.AttackRange;
            if ( !_inAttackRange ) Debug.Log( "out of range" );
            pAnimator.SetBool( "InAttackRange", _inAttackRange );
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _t = 0;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void AttackUsing( WizardAI wizardAI ) {
        _t -= Time.deltaTime;
        Debug.Log( _t );
        if ( _t <= 0 ) {
            _t = _cooldown;

            wizardAI.InstantiateAttack(_aiController.Target.transform);
        }
    }
}

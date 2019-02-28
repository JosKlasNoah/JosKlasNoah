using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ai_Sense_SeeTarget_Behaviour : StateMachineBehaviour {
    private Ray[] _rayList;
    private AiController _aiController;
    private Transform _aiTransform;
    private Vector2 _rays;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        _aiController = animator.gameObject.GetComponent<AiController>();
        SetRays();
        _aiTransform = animator.transform;
    }

    public void SetRays()
    {
        Debug.Assert( _aiController != null, "No aiController found yet" );
        _rays = new Vector2(Mathf.RoundToInt(_aiController.Rays.x), Mathf.RoundToInt(_aiController.Rays.y));
        _rayList = new Ray[ ( int )( _rays.x * _rays.y ) ];
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex ) {;
        Look();
        
        foreach ( Ray ray in _rayList ) {
            FindWith( ray );
            Draw( ray );
        }
    }

    private void Look()
    {
        float xAmountToSide = _rays.x / 2f - 0.5f;
        
        if ( _rays.x != 0 && _rays.y != 0 )
            for ( int i = 0; i < (int)_rays.x; i++ )
            {
                for ( int j = 0; j < (int)_rays.y; j++ )
                {
                    //set rays
                    _rayList[ i * ( int )_rays.y + j ] = new Ray( _aiTransform.position + _aiTransform.up * _aiController.EyeHeight, ( _aiTransform.forward + ( _aiTransform.up * _aiController.EyeSight.y * ( j - 1 ) ) + ( _aiTransform.right * _aiController.EyeSight.x * ( i - xAmountToSide ) ) ).normalized * _aiController.LookDepth );
                }
            }
        else {
            Debug.Log( "Can't draw 0 rays" );
        }
    }

    private void FindWith( Ray pRay ) {
        RaycastHit hit;

        if ( Physics.Raycast( pRay, out hit ) ) {
            //Should create a better solution for this

            PlayerController FoundItem = hit.collider.gameObject.GetComponent<PlayerController>();

            if ( FoundItem != null ) {
                // Found Target
                _aiController.Target = FoundItem;
            }
            else {
                // Didn't find target
                //Debug.Log( hit.collider.gameObject );
                if ( _aiController.Target != null && Vector3.Distance( _aiTransform.position, _aiController.Target.transform.position ) > _aiController.LoseTargetDistance ) {
                    _aiController.LostTargetActor();
                }
            }
        }
    }

    private void Draw( Ray pRay )
    {
        Debug.DrawRay( pRay.origin, pRay.direction * _aiController.LookDepth, Color.red );
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

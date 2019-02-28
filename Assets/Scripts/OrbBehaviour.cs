using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

[RequireComponent( typeof( Rigidbody ) )]
public class OrbBehaviour : ObjectBase {
    [SerializeField, Range( 0, 100 )]
    private float _speed = 30;

    private Rigidbody _rigidBody;
    private Transform _transformToFace;
    private GameObject _whirls;
    private bool _activated;


    private void Start() {
        _rigidBody = GetComponent<Rigidbody>();
        _transformToFace = GameManager.CurrentPlayerController.transform;
        _whirls = transform.GetChild(1).GetChild(2).gameObject;
        Debug.AssertFormat( _rigidBody, "No RigidBody on the object" );
        Debug.Assert( _whirls, "Collection object of Whirls special effect missing or moved!" );
        Debug.Assert( _transformToFace, "Equip a camera transform towards which the whirls should face!" );
    }

    private void Update() {
        Vector3 fromToVector = _transformToFace.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation( fromToVector ) * Quaternion.Euler( 45, 0, 45 );
        _whirls.transform.rotation = rotation;
    }

    public void Activate() {
        _activated = true;
        SwitchColliders();

        gameObject.layer = 2;
        _rigidBody.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter( Collider other ) {

        if ( other.GetComponent<WizardAI>() && _activated ) {
            _activated = false;
            SwitchColliders();

            gameObject.layer = 0;
            other.GetComponent<WizardAI>().Disintegrate();
        }
    }

    private void SwitchColliders() {
        foreach ( Collider col in GetComponents<Collider>() ) {
            col.enabled = !col.enabled;
        }
    }
}
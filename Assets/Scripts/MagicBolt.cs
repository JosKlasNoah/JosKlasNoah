using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MagicBolt : MonoBehaviour
{
    [SerializeField, Range( 0, 20 )]
    private float _speed = 10.5f;

    [SerializeField, Range(5, 10)]
    private float _expirationTime = 7;

    private ParticleSystem[] _particleSystem;
    private Transform _transformToFollow;
    private Rigidbody _rigidBody;
    private Transform _meshObject;
    private Transform _effectObject;
    private SphereCollider _trigger;

    private int _particleSystemCount = 4;
    private float _effectTime = 1.07f;

    public Transform TransformToFollow {
        get => _transformToFollow;
        set {
            _transformToFollow = value;
        }
    }


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _trigger = GetComponents<SphereCollider>()[1];
        Debug.AssertFormat( null != _rigidBody, "No RigidBody on the object" );
        Debug.AssertFormat( null != _trigger || ( null != _trigger && !_trigger.isTrigger ), "No trigger Collider on the object" );

        _rigidBody.velocity = ( ( _transformToFollow.position + new Vector3( 0, _transformToFollow.localScale.y * 0.75f, 0 ) ) - transform.position ).normalized * _speed;

        _meshObject = transform.GetChild(0);
        _effectObject = transform.GetChild(1);
        
        _particleSystemCount = _effectObject.childCount;
        _particleSystem = new ParticleSystem[ _particleSystemCount ];

        
        for ( int i = 0; i < _particleSystemCount; i++ ) {
            _particleSystem[ i ] = _effectObject.GetChild(i).GetComponent<ParticleSystem>();
        }

        Destroy( _meshObject.gameObject, _expirationTime - _effectTime );
        Destroy( _effectObject.gameObject, _expirationTime - _effectTime );
        Destroy( this.gameObject, _expirationTime );
    }

    private void OnCollisionEnter( Collision other )
    {
        _rigidBody.velocity = Vector3.zero;
        _trigger.enabled = false;

        for ( int i = 0; i < _particleSystemCount; i++ )
        {
            if ( null != _particleSystem[ i ] )
            {
                if ( i == 0 )
                {
                    // glow
                    Destroy(_particleSystem[ i ].gameObject);
                }
                else
                {
                    // lightning, sparks, and anything else
                    _particleSystem[ i ].Stop();
                }
            }
        }

        if ( null != _meshObject )
            Destroy(_meshObject.gameObject);
        if ( null != _effectObject )
            Destroy(_effectObject.gameObject, _effectTime);
        if ( null != this.gameObject )
            Destroy(this.gameObject, _effectTime);
    }
}

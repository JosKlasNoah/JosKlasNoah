using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MagicBolt : MonoBehaviour
{
    [SerializeField, Range( 0, 10 )]
    private float _speed = 5.5f;

    [SerializeField, Range(5, 10)]
    private float _expirationTime = 7;

    private Transform _transformToFollow;
    private Rigidbody _rigidBody;

    public Transform TransformToFollow {
        get => _transformToFollow;
        set {
            _transformToFollow = value;
        }
    }


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.velocity = ( _transformToFollow.position - transform.position ).normalized * _speed;

        Destroy( this.gameObject, _expirationTime );
    }
}

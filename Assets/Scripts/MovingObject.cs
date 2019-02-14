using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public class MovingObject : MonoBehaviour
{
    [SerializeField, Tooltip( "Distance in units the object travels on each axis in world space" )]
    private Vector3 _range;

    [SerializeField, Tooltip("The speed at which the object travels on each axis in world space")]
    private float _speed;

    [SerializeField, Tooltip("Time in seconds how far into the movement the object starts")]
    private float _timeOffset;

    private Vector3 _startPosition, _targetPosition;
    private Rigidbody _rb;


    private void Start()
    {
        Debug.Assert( null != GetComponent<Rigidbody>(), "Object doesn't have a rigidbody" );
        _rb = GetComponent<Rigidbody>();
    }

    private void OnValidate()
    {
        _startPosition = transform.position;
        _targetPosition = _startPosition + _range * 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color( 0, 1, 0, 0.15f );
        Gizmos.DrawSphere( _startPosition, 0.175f );
        Gizmos.color = new Color( 1, 0, 0, 0.15f );
        Gizmos.DrawSphere( _targetPosition, 0.175f );
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector3( Mathf.Sin( ( Time.fixedTime + _timeOffset ) * _speed ) * _speed * _range.x, Mathf.Sin( ( Time.fixedTime + _timeOffset ) * _speed ) * _speed * _range.y, Mathf.Sin( ( Time.fixedTime + _timeOffset ) * _speed ) * _speed * _range.z );
    }
}

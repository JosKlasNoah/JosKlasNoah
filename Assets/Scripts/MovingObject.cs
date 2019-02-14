using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingObject : MonoBehaviour
{
    [SerializeField, Tooltip("Distance in units the object travels on each axis in world space")]
    private Vector3 _range;

    [SerializeField, Tooltip("The speed at which the object travels on each axis in world space")]
    private Vector3 _speed;

    [SerializeField, Tooltip("Time in seconds how far into the movement the object starts")]
    private Vector3 _timeOffset;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Rigidbody _rb;


    private void Start()
    {
        Debug.Assert( null != GetComponent<Rigidbody>(), "Object doesn't have a rigidbody" );
        _rb = GetComponent<Rigidbody>();
    }

    private void OnValidate()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position + new Vector3( _speed.x * _range.x, _speed.y * _range.y, _speed.z * _range.z ) * .9f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color( 0, 1, 0, 0.5f );
        Gizmos.DrawSphere( _startPosition, 0.2f );
        Gizmos.DrawSphere( _targetPosition, 0.2f );
    }

    void FixedUpdate()
    {
        //transform.position = new Vector3( _startPosition.x + Mathf.Sin( ( Time.time + _timeOffset.x ) * _speed.x ) * _range.x, _startPosition.y + Mathf.Sin( ( Time.time + _timeOffset.y ) * _speed.y ) * _range.y, _startPosition.z + Mathf.Sin( ( Time.time + _timeOffset.z ) * _speed.z ) * _range.z );
        
        _rb.velocity = new Vector3( Mathf.Sin( ( Time.time + _timeOffset.x ) * _speed.x ) * _range.x, Mathf.Sin( ( Time.time + _timeOffset.y ) * _speed.y ) * _range.y, Mathf.Sin( ( Time.time + _timeOffset.z ) * _speed.z ) * _range.z );
        Debug.Log( _rb.velocity );
    }
}

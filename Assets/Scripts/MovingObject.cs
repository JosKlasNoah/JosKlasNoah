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
    private Rigidbody _rb;


    private void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(_startPosition.x + Mathf.Sin((Time.time + _timeOffset.x) * _speed.x) * _range.x, _startPosition.y + Mathf.Sin((Time.time + _timeOffset.y) * _speed.y) * _range.y, _startPosition.z + Mathf.Sin((Time.time + _timeOffset.z) * _speed.z) * _range.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveBolt : MonoBehaviour
{
    [SerializeField]
    private float _range = 2;

    [SerializeField]
    private float _rate = 0.25f;

    private Vector3 _startPosition;


    private void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3( _startPosition.x + Mathf.Sin( Time.time / _rate) * _range, _startPosition.y, _startPosition.z );
    }
}

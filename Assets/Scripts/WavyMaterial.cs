using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( MeshFilter ) )]
public class WavyMaterial : MonoBehaviour
{
    [SerializeField]
    private float _amplitude;

    [SerializeField]
    private float _speed;

    private MeshFilter _filter;

    void Start()
    {
        _filter = GetComponent<MeshFilter>();
    }
    
    void FixedUpdate()
    {
        Vector3[] vertices = _filter.mesh.vertices;

        for ( int i = 0; i < vertices.Length; i++ )
        {
            vertices[ i ] += Vector3.up * Mathf.Sin( ( Time.fixedTime ) * _speed ) * _speed * _amplitude;
        }
    }
}

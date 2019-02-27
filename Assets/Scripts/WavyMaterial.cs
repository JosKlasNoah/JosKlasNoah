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
    private Vector3[] _vertices;

    void Start()
    {
        _filter = GetComponent<MeshFilter>();
        _vertices = _filter.mesh.vertices;
    }
    
    void FixedUpdate()
    {
        Debug.LogFormat("amount of vertices: {0}.", _vertices.Length);
        
        for ( int i = 1; i < _vertices.Length; i += 3 )
        {
            Debug.LogFormat("vertice index: {0}.", i);
            _vertices[ i ] += Vector3.up * Mathf.Cos(( Time.fixedTime ) * _speed) * _speed * _amplitude;
            //vertices[ i + 1 ] += Vector3.up * Mathf.Cos(( Time.fixedTime ) * _speed) * _speed * _amplitude;
            _vertices[ i + 2 ] += Vector3.up * Mathf.Cos(( Time.fixedTime ) * _speed) * _speed * _amplitude;
        }

        _filter.mesh.vertices = _vertices;
    }
}

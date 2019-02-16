using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    [SerializeField]
    private Transform _transformToFace;

    [SerializeField]
    private Color _color;

    private Material _orbMaterial;
    private GameObject _whirls;
    private float hue;

    private void Start() {
        _orbMaterial = GetComponentInChildren<MeshRenderer>().material;
        Debug.Assert( null != _orbMaterial, "No MeshRenderer found in children!" );
        _whirls = transform.GetChild(1).GetChild(2).gameObject;
        Debug.Assert( null != _whirls, "Collection object of Whirls special effect missing or moved!" );
    }

    private void Update() {
        ColorChange();
        FaceTransform();
    }

    private void ColorChange() {
        hue++;

        //_color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        //_orbMaterial.color = _color;
    }

    private void FaceTransform() {
        Vector3 fromToVector = _transformToFace.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation( fromToVector ) * Quaternion.Euler( 45, 0, 45 );
        _whirls.transform.rotation = rotation;
    }
}

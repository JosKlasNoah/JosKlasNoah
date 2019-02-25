using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    [SerializeField]
    private Transform _transformToFace;

    private GameObject _whirls;


    private void Start()
    {
        _whirls = transform.GetChild(1).GetChild(2).gameObject;
        Debug.Assert(null != _whirls, "Collection object of Whirls special effect missing or moved!");
    }

    private void Update()
    {
        Vector3 fromToVector = _transformToFace.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(fromToVector) * Quaternion.Euler(45, 0, 45);
        _whirls.transform.rotation = rotation;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : MonoBehaviour {
    [SerializeField]
    private GameObject _attackPrefab;

    [SerializeField]
    private GameObject _disintegrationPrefab;

    [SerializeField]
    private Vector2 _attackOffset;

    [SerializeField]
    private Transform _checkPoint;

    [SerializeField]
    private float _expirationTime = 12;


    public void InstantiateAttack( Transform pTarget ) {
        Vector3 offset = transform.position + transform.forward * _attackOffset.x + Vector3.up * _attackOffset.y;
        MagicBolt attack = Instantiate( _attackPrefab, offset, Quaternion.identity ).GetComponent<MagicBolt>();

        attack.TransformToFollow = pTarget;
        attack.CheckPoint = _checkPoint;
    }
    
    public void Disintegrate() {

        GameObject wizintegration = Instantiate( _disintegrationPrefab, transform,true );
        wizintegration.transform.SetParent(null);
        wizintegration.SetActive( true );

        CopyMeshPositionRotation( wizintegration );
        
        Destroy( gameObject );
        Destroy( wizintegration, _expirationTime );
    }

    private void CopyMeshPositionRotation( GameObject pWizintegration ) {
        int childCount = transform.childCount;
        Transform[] childMeshObjects = new Transform[ childCount ];
        Transform wizMeshObject = pWizintegration.transform.GetChild(0);
        Transform[] wizChildMeshObjects = new Transform[ childCount ];

        for ( int i = 0; i < childCount; i++ ) {
            childMeshObjects[ i ] = transform.GetChild( i );
            wizChildMeshObjects[ i ] = wizMeshObject.GetChild( i );

            wizChildMeshObjects[ i ].localPosition = childMeshObjects[ i ].localPosition;
            wizChildMeshObjects[ i ].localRotation = childMeshObjects[ i ].localRotation;
        }
    }

    private void OnCollisionEnter( Collision collision ) {
        PlayerHandler.SetPlayerTransform( _checkPoint.gameObject );
    }
}
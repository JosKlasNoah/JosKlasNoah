using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
    [SerializeField]
    private int _loadingTime = 3;

    private ParticleSystem[] _particleSystem;
    GameObject _ammo;

    private int _particleSystemCount = 3;

    private IEnumerator _cannonTimer;


    public void Load( GameObject pAmmo )
    {
        _ammo = pAmmo;
        StartCoroutine( Loading( _loadingTime ) );

        Transform effectObject = transform.GetChild( 1 );
        _particleSystemCount = effectObject.childCount;
        _particleSystem = new ParticleSystem[ _particleSystemCount ];

        for ( int i = 0; i < _particleSystemCount; i++ ) {
            _particleSystem[ i ] = effectObject.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    private IEnumerator Loading( int ploadingTime )
    {
        Debug.LogFormat( "{0}...", _loadingTime );
        if ( _loadingTime <= 0 ) {
            Fire();
            yield return null;
        }
        else {
            yield return new WaitForSeconds( 1 );
            StartCoroutine( Loading( --_loadingTime ) );
        }

    }

    private void Fire() {
        Debug.Log( "Fire!" );
        Transform aimObject = transform.GetChild( 3 );

        _ammo.transform.position = aimObject.transform.position;
        _ammo.transform.rotation = aimObject.transform.rotation;
        _ammo.SetActive( true );
        _ammo.GetComponent<OrbBehaviour>().Activate();

        _particleSystem[ 0 ].Play();
        _particleSystem[ 1 ].Play();
    }
}

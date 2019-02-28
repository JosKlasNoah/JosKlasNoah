using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _attackPrefab;

    [SerializeField]
    private Vector2 _offset;

    public void InstantiateAttack( Transform pTarget )
    {
        Vector3 offset = transform.position + transform.forward * _offset.x + Vector3.up * _offset.y;
        GameObject attack = Instantiate( _attackPrefab, offset, Quaternion.identity );

        // this is bad practise, but it's a fast fix for now
        attack.GetComponent<MagicBolt>().TransformToFollow = pTarget;
    }
}

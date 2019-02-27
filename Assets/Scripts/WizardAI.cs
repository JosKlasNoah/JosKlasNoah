using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _attackPrefab;

    [SerializeField]
    private Vector2 _offset;

    public void InstantiateAttack()
    {
        Vector3 offset = transform.position + transform.forward * _offset.x + Vector3.up * _offset.y;
        Instantiate( _attackPrefab, offset, Quaternion.identity );
    }
}

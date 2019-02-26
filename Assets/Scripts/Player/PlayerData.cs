using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Movement")]
    [SerializeField]
    public bool _canMove = true;
    [SerializeField]
    public float _moveSpeed = 2;
    [SerializeField]
    public bool _canRun = true;
    [SerializeField, Range(0, 1)]
    public float _sprintSpeed = .2f;
    [SerializeField]
    public bool _canCrouch = true;
    [SerializeField, Range(0, 1)]
    public float _crouchSpeed = .8f;



    [SerializeField, Header("Jumping")]
    public bool _canJump = true;
    [SerializeField]
    public float _jumpHeight = 5;
    [SerializeField]
    public int _maxJumpCount = 1;
    [SerializeField]
    public float _jumpDelay = 0f;
    [SerializeField, Range(0, 1)]
    public float _airSpeed = .1f;

    [SerializeField, Header("Height")]
    public float _normalHeight = 2.2f;
    [SerializeField]
    public float _crouchHeight = 1.5f;

    [SerializeField, Header("ObjectInteraction")]
    public float _objectInteractionDistance = 2.2f;

    [SerializeField, Header("Raycasts")]
    public LayerMask _rayCastLayers = new LayerMask();

    [SerializeField, Header("Debug")]
    public bool _debugRays = false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

[CreateAssetMenu(fileName = "Custom / PlayerConfig")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField]
    PlayerData _playerData = new PlayerData();

    [Header("Debug"), SerializeField]
    public float mouseSpeed = 3f;

    public PlayerData playerData
    {
        get
        {
            PlayerData temp = new PlayerData();

            temp._airSpeed = _playerData._airSpeed;
            temp._canCrouch = _playerData._canCrouch;
            temp._canJump = _playerData._canJump;
            temp._canMove = _playerData._canMove;
            temp._canRun = _playerData._canRun;
            temp._crouchHeight = _playerData._crouchHeight;
            temp._crouchSpeed = _playerData._crouchSpeed;
            temp._debugRays = _playerData._debugRays;
            temp._jumpDelay = _playerData._jumpDelay;
            temp._jumpHeight = _playerData._jumpHeight;
            temp._maxJumpCount = _playerData._maxJumpCount;
            temp._moveSpeed = _playerData._moveSpeed;
            temp._normalHeight = _playerData._normalHeight;
            temp._objectInteractionDistance = _playerData._objectInteractionDistance;
            temp._sprintSpeed = _playerData._sprintSpeed;

            return temp;
        }
    }

}

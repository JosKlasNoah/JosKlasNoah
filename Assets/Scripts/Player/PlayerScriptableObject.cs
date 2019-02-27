using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

[CreateAssetMenu(fileName = "Custom / PlayerConfig")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField]
    public PlayerData _playerData = new PlayerData();

    [Header("Debug"), SerializeField]
    public float mouseSpeed = 3f;

}

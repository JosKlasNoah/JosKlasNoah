using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    Vector3 _spawnPos;

    [SerializeField]
    Vector3 _spawnRot;

    public void SpawnNewPlayer()
    {
        // Create a custom game object
        GameObject go = new GameObject("Player");
        go.AddComponent<PlayerController>();
        go.AddComponent<AudioPlayerManager>();
        go.transform.position = _spawnPos;
        go.transform.rotation = Quaternion.Euler(_spawnRot);
    }
}

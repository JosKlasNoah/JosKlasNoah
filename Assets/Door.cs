using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool _isDoorOpen;
    Vector3 originalDoorRotation;
    [SerializeField]
    float _openAmount = 90f;

    private void Awake()
    {
        originalDoorRotation = transform.rotation.eulerAngles;
    }


    public void OpenDoor(bool open)
    {
        if (open != _isDoorOpen)
        {
            transform.rotation = Quaternion.Euler(originalDoorRotation += Vector3.up * (open ? 1 : -1) * _openAmount);
            _isDoorOpen = open;
        }
    }


}

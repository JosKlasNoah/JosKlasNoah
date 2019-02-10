using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

public class PlayerHandler : MonoBehaviour
{

    public void SetJumpCount(int count)
    {
        GameManager.CurrentPlayerController.gameObject.GetComponent<PlayerController>().JumpCount = count;
    }

    public void DisablePlayer()
    {
        GameManager.CurrentPlayerController.enabled = false;
        // GameManager.CurrentPlayerController.transform.GetComponentInChildren<Camera>().enabled = false;
    }

    public void EnablePlayer()
    {
        GameManager.CurrentPlayerController.enabled = true;
        //GameManager.CurrentPlayerController.transform.GetComponentInChildren<Camera>().enabled = true;
    }

    public void EnableCamera(bool pEnable)
    {
        GameManager.CurrentPlayerController.Cam.cullingMask = pEnable ? 1 : 0;
    }
}

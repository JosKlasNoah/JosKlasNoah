using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

public class PlayerHandler : MonoBehaviour
{

    public void DisablePlayer()
    {
        GameManager.CurrentPlayerController.gameObject.GetComponent<PlayerController>().enabled = false;
        GameManager.CurrentPlayerController.gameObject.transform.GetComponentInChildren<Camera>().enabled = false;
    }

    public void EnablePlayer()
    {
        GameManager.CurrentPlayerController.gameObject.GetComponent<PlayerController>().enabled = true;
        GameManager.CurrentPlayerController.gameObject.transform.GetComponentInChildren<Camera>().enabled = true;
    }
}

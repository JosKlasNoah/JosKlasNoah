using Custom.GameManager;
using UnityEngine;

public class PlayerHandler
{
    public static void SetJumpCount(int count)
    {
        GameManager.CurrentPlayerController.gameObject.GetComponent<PlayerController>().JumpCount = count;
    }

    public static void MovePlayer(Vector3 pos)
    {
        GameManager.CurrentPlayerController.transform.position = pos;
    }

    public static void DisablePlayer()
    {
        GameManager.CurrentPlayerController.enabled = false;
    }

    public static void EnablePlayer()
    {
        GameManager.CurrentPlayerController.enabled = true;
        //GameManager.CurrentPlayerController.transform.GetComponentInChildren<Camera>().enabled = true;
    }

    public static void EnableCamera(bool pEnable)
    {
        GameManager.CurrentPlayerController.Cam.cullingMask = pEnable ? 1 : 0;
    }
}

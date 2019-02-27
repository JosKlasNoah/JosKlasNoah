using Custom.GameManager;
using UnityEngine;

public class PlayerHandler
{

    static PlayerController Player => GameManager.CurrentPlayerController;
    public static void SetJumpCount(int count)
    {
        Player.JumpCount = count;
    }

    public static void MovePlayer(Vector3 pos)
    {
        Player.transform.position = pos;
    }

    public static void MovePlayerWithGameObject(GameObject obj)
    {
        Player.transform.position = obj.transform.position;
    }

    public static void SetPlayerEnabled(bool Enabled)
    {
        Player.enabled = Enabled;
    }

    public static void EnableCamera(bool pEnable)
    {
        Player.Cam.cullingMask = pEnable ? 1 : 0;
    }

    public static void CanPlayerMove(bool CanMove)
    {
        Player._playerData._canMove = CanMove;
    }

    public static void CanPlayerJump(bool CanJump)
    {
        Player._playerData._canJump = CanJump;
    }
    public static void CanPlayerCrouch(bool CanCrouch)
    {
        Player._playerData._canCrouch = CanCrouch;
    }
    public static void CanPlayerSprint(bool CanSprint)
    {
        Player._playerData._canRun = CanSprint;
    }
}

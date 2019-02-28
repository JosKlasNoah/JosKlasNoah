using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

public class CannonEvents {
    public static void LoadCannon( GameObject obj )
    {
        Debug.Log( "Loading..." );
        PlayerController playerController = GameManager.CurrentPlayerController;
        GameObject holdingObject = playerController.holdingObject.GetGameObject();

        playerController.holdingObject.OnItemInteract( playerController );
        holdingObject.SetActive( false );

        obj.GetComponent<Cannon>().Load( holdingObject );
    }
}

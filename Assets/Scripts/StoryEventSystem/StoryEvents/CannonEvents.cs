using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEvents
{
    public static void Load( GameObject obj ) {
        Debug.Log( "Loading..." );
        obj.GetComponent<Cannon>();
    }

    private static void Fire() {
        Debug.Log( "Fire!" );

    }
}

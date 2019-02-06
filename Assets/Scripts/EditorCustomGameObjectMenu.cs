using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorCustomGameObjectMenu
{


    [MenuItem("GameObject/Custom", false, 1), MenuItem("GameObject/Custom/Player", false, 0)]
    static void CreatePlayerObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Player");
        go.AddComponent<PlayerController>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}

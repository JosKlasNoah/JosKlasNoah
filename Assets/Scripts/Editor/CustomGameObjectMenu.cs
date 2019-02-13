using UnityEditor;
using UnityEngine;

public class CustomGameObjectMenu
{
    [MenuItem("GameObject/Custom", false, 1), MenuItem("GameObject/Custom/Player", false, 0)]
    static void CreatePlayerObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Player");
        go.AddComponent<PlayerController>();
        go.AddComponent<AudioPlayerManager>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Custom/GameEvent", false, 15)]
    static void CreateGameEventTriggerObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("GameEvent");
        go.AddComponent<StoryEvent>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}

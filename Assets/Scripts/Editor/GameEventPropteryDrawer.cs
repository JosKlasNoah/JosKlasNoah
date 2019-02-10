using Custom.Story;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StoryEventContainer))]
public class GameEventPropteryDrawer : PropertyDrawer
{
    Dictionary<TriggerType, float> TriggerTypeHeight = new Dictionary<TriggerType, float>()
        {
            {TriggerType.Awake,50 },
            {TriggerType.Player,80 }
        };

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return base.CanCacheInspectorGUI(property);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;

        TriggerType _tt = GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType"));
        if (TriggerTypeHeight.ContainsKey(_tt))
        {
            height = TriggerTypeHeight[_tt];
        }
        else
        {
            height = 500;
        }

        height += property.FindPropertyRelative("StoryEventsToPlay").arraySize * 25;

        return height;
    }

    string[] options = System.Enum.GetNames(typeof(TriggerType));
    Vector2 CalculativeRect;
    Rect GuiRect;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GuiRect = position;
        CalculativeRect = new Vector2(position.x, position.y);

        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(CalculateRect(position.width , 0,true), "", GUI.skin.horizontalSlider);
        CalculateRect(0, 5, 0, 0);

        EditorGUI.PrefixLabel(CalculateRect(70, 0), new GUIContent("EventName:"));
        EditorGUI.PropertyField(CalculateRect(100, 10), property.FindPropertyRelative("_eventName"), GUIContent.none);

        EditorGUI.PrefixLabel(CalculateRect(70, 5), new GUIContent("EventType:"));
        EditorGUI.PropertyField(CalculateRect(100, 0), property.FindPropertyRelative("_storyEventTriggerType"), GUIContent.none);

        switch (GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType")))
        {
            case TriggerType.Awake:
                DisplayAwakePropertys(position, property, label);
                break;

            case TriggerType.Player:
                DisplayPlayerPropertys(position, property, label);
                break;

            default:
                DisplayAllPropertys(position, property, label);
                Debug.LogWarning("TriggerType has no custom UI implemented");
                break;
        }

        EditorGUI.PrefixLabel(CalculateRect(120, 200), new GUIContent("StoryEventAmount:"));
        property.FindPropertyRelative("StoryEventsToPlay").arraySize = EditorGUI.IntField(CalculateRect(50, 0), property.FindPropertyRelative("StoryEventsToPlay").arraySize);


        SerializedProperty tempProp = property.FindPropertyRelative("StoryEventsToPlay");
        for (int i = 0; i < tempProp.arraySize; i++)
        {
            NextLine(25);
            EditorGUI.PropertyField(CalculateRect(150, 50), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_audioToPlay"), GUIContent.none, true);
            EditorGUI.PropertyField(CalculateRect(50,0), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("t"), GUIContent.none,true);
        }

        NextLine(30);
        //EditorGUI.PropertyField(CalculateRect(position.width,250,0,0), property.FindPropertyRelative("StoryEventsToPlay"),true);

        
        EditorGUI.EndProperty();
    }

    void DisplayAwakePropertys(Rect position, SerializedProperty property, GUIContent label)
    {

    }

    void DisplayPlayerPropertys(Rect position, SerializedProperty property, GUIContent label)
    {
        NextLine(20);

        EditorGUI.PrefixLabel(CalculateRect(140, 35), new GUIContent("InteractionCount:    Min:"));
        EditorGUI.PropertyField(CalculateRect(50, 10), property.FindPropertyRelative("_interactionCountBeforePlay"), GUIContent.none);
        EditorGUI.PrefixLabel(CalculateRect(30, 0), new GUIContent("Max:"));
        EditorGUI.PropertyField(CalculateRect(50, 10), property.FindPropertyRelative("_maxInteractionCount"),GUIContent.none);

        //EditorGUI.PropertyField(CalculateRect(100, 0), property.FindPropertyRelative("_storyEventTriggerType"), GUIContent.none);

    }

    void DisplayAllPropertys(Rect position, SerializedProperty property, GUIContent label)
    {
        //  EditorGUI.PropertyField(position.);
    }


    TriggerType GetPropertyAsTriggerType(SerializedProperty property)
    {
        return (TriggerType) System.Enum.Parse(typeof(TriggerType), options[property.enumValueIndex]);
    }

    Rect CalculateRect(float Pwidth, float Pheight, float PspaceWidth, float PspaceHeight, bool pIgnoreHeightChange = false)
    {
        if (CalculativeRect.x + PspaceWidth + Pwidth > GuiRect.width)
        {
            CalculativeRect.x = GuiRect.x;
            CalculativeRect.y += 20;
        }

        Rect TempRect = new Rect(CalculativeRect.x + PspaceWidth, CalculativeRect.y + PspaceHeight, Pwidth, Pheight);

        CalculativeRect = new Vector2(CalculativeRect.x + Pwidth + PspaceWidth, CalculativeRect.y + (pIgnoreHeightChange ? 0 : (Pheight + PspaceHeight)));

        return TempRect;
    }


    Rect CalculateRect(float Pwidth, float PspaceWidth, bool pIgnoreHeightChange = false)
    {
        if (CalculativeRect.x + PspaceWidth + Pwidth > GuiRect.width &&!pIgnoreHeightChange)
        {
            CalculativeRect.x = GuiRect.x -5;
            CalculativeRect.y += 20;
        }

        Rect TempRect = new Rect(CalculativeRect.x + PspaceWidth, CalculativeRect.y, Pwidth, 20);

        CalculativeRect = new Vector2(CalculativeRect.x + Pwidth + PspaceWidth, CalculativeRect.y);

        return TempRect;
    }



    Rect NextLine(float Pheight)
    {
        Rect TempRect = new Rect(0, CalculativeRect.y + Pheight, 0, Pheight);

        CalculativeRect = new Vector2(0, CalculativeRect.y + Pheight);

        return TempRect;
    }

}
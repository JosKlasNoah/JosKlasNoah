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
            {TriggerType.Trigger,80 }
        };

    string[] options = System.Enum.GetNames(typeof(TriggerType));

    Vector2 CalculativeRect;
    Rect GuiRect;

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return base.CanCacheInspectorGUI(property);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 500;

        TriggerType _tt = GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType"));
        if (TriggerTypeHeight.ContainsKey(_tt))
        {
            height += TriggerTypeHeight[_tt];
        }
        else
        {
            height += 500;
        }

        height += property.FindPropertyRelative("StoryEventsToPlay").arraySize * 25;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GuiRect = position;
        CalculativeRect = new Vector2(position.x, position.y);
        EditorGUI.BeginProperty(position, label, property);



        List<System.Reflection.MethodInfo> atemp = StoryDelegate.GetAllMethods();

        List<string> ctemp = StoryDelegate.MethodsToString(atemp);


        SerializedProperty tempBase = property.FindPropertyRelative("_storyDel");
        EditorGUI.LabelField(CalculateRect(150, 0), tempBase.FindPropertyRelative("_functionName").stringValue);

        NextLine(25);

        List<string> btemp = new List<string>() { "a/b", "g", "a/c" };

        //ClassName DropDownMenu
        int _menuIndex = EditorGUI.Popup(CalculateRect(150, 0), 2, btemp.ToArray());

      //  tempBase.FindPropertyRelative("_className").stringValue = StoryEventManager.GetEventExecutionMethodsAsString()[_menuIndex];


        int _oldStringIndex = ctemp.IndexOf(tempBase.FindPropertyRelative("_functionName").stringValue);
        _oldStringIndex = (_oldStringIndex) < 0 ? 0 : _oldStringIndex;

        int _newStringIndex = EditorGUI.Popup(CalculateRect(150, 0), _oldStringIndex, ctemp.ToArray());

        if (_newStringIndex != _oldStringIndex)
        {
            System.Reflection.MethodInfo foundMethod = atemp[_newStringIndex];

            tempBase.FindPropertyRelative("_functionName").stringValue = foundMethod.Name;

            tempBase = tempBase.FindPropertyRelative("_functionParameters");
            tempBase.ClearArray();

            System.Type[] parameterTypes = StoryDelegate.GetMethodParameterTypes(foundMethod);

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                NextLine(25);
                tempBase.InsertArrayElementAtIndex(i);
                SerializedProperty currentElement = tempBase.GetArrayElementAtIndex(i);
                SetTypeObjectWrapperValue(currentElement, parameterTypes[i]);
            }
        }
        else
        {
            tempBase = tempBase.FindPropertyRelative("_functionParameters");

            for (int i = 0; i < tempBase.arraySize; i++)
            {
                NextLine(25);
                SerializedProperty currentElement = tempBase.GetArrayElementAtIndex(i);
                DrawPropertyForCustomType(CalculateRect(200, 0), currentElement);
            }
        }



        #region temp hidden
        /*
        NextLine(30);
        EditorGUI.LabelField(CalculateRect(position.width, 0, true), "", GUI.skin.horizontalSlider);

        EditorGUI.PrefixLabel(CalculateRect(70, 0), new GUIContent("EventName:"));
        EditorGUI.PropertyField(CalculateRect(150, 10), property.FindPropertyRelative("_eventName"), GUIContent.none);

        NextLine(25);

        EditorGUI.PrefixLabel(CalculateRect(60, 15), new GUIContent("EventType:"));
        EditorGUI.PropertyField(CalculateRect(150, 20), property.FindPropertyRelative("_storyEventTriggerType"), GUIContent.none);

        switch (GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType")))
        {
            case TriggerType.Awake:
                DisplayAwakePropertys(position, property, label);
                break;

            case TriggerType.Trigger:
                DisplayTriggerPropertys(position, property, label);
                NextLine(20);
                break;

            default:
                DisplayAllPropertys(position, property, label);
                Debug.LogWarning("TriggerType has no custom UI implemented");
                break;
        }


        NextLine(20);
        EditorGUI.PrefixLabel(CalculateRect(140, 30), new GUIContent("RequiredEventCount:"),GUIStyle.none);
        property.FindPropertyRelative("_eventRequiredmentList").arraySize = Mathf.Clamp(EditorGUI.IntField(CalculateRect(30, 10), property.FindPropertyRelative("_eventRequiredmentList").arraySize), 0, 5);
        SerializedProperty tempProp = property.FindPropertyRelative("_eventRequiredmentList");

        if (tempProp.arraySize > 0)
        {
            NextLine(25);

            //_eventRequiredmentList
            EditorGUI.LabelField(CalculateRect(80, 30), new GUIContent("Completed"));
            EditorGUI.LabelField(CalculateRect(85, 45), new GUIContent("EventName"));
        }

        for (int i = 0; i < tempProp.arraySize; i++)
        {
            NextLine(25);

            EditorGUI.PropertyField(CalculateRect(50, 55), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_Completed"), GUIContent.none);
            EditorGUI.PropertyField(CalculateRect(100, 45), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_eventName"), GUIContent.none);
        }
        NextLine(40);

        EditorGUI.PrefixLabel(CalculateRect(150, 20), new GUIContent("StoryEventAmount:"), GUIStyle.none);
        property.FindPropertyRelative("StoryEventsToPlay").arraySize = Mathf.Clamp(EditorGUI.IntField(CalculateRect(50, 0), property.FindPropertyRelative("StoryEventsToPlay").arraySize), 0, 50);


        tempProp = property.FindPropertyRelative("StoryEventsToPlay");
        for (int i = 0; i < tempProp.arraySize; i++)
        {
            NextLine(25);
            EditorGUI.PropertyField(CalculateRect(150, 50), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_audioToPlay"), GUIContent.none, true);


        }

    */
        #endregion
        EditorGUI.EndProperty();
    }


    void DrawPropertyForCustomType(Rect trans, SerializedProperty objectWrapper)
    {
        TypeObjectWrapper.valueType vt = (TypeObjectWrapper.valueType) objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex;

        switch (vt)
        {
            case TypeObjectWrapper.valueType.Int:
                objectWrapper.FindPropertyRelative("_int").intValue = EditorGUI.IntField(trans, objectWrapper.FindPropertyRelative("_int").intValue);
                break;
            case TypeObjectWrapper.valueType.Float:
                objectWrapper.FindPropertyRelative("_float").floatValue = EditorGUI.FloatField(trans, objectWrapper.FindPropertyRelative("_float").floatValue);
                break;
            case TypeObjectWrapper.valueType.Bool:
                objectWrapper.FindPropertyRelative("_bool").boolValue = EditorGUI.Toggle(trans, objectWrapper.FindPropertyRelative("_bool").boolValue);
                break;
            case TypeObjectWrapper.valueType.UnityObject:
                objectWrapper.FindPropertyRelative("_unityObject").objectReferenceValue = EditorGUI.ObjectField(trans, (GameObject) objectWrapper.FindPropertyRelative("_unityObject").objectReferenceValue, typeof(GameObject), true);
                break;
            default:
                Debug.LogWarning("shit aint implemented yet");
                return;
        }
    }

    object GetPropertyValueForCustomType(SerializedProperty objectWrapper)
    {
        TypeObjectWrapper.valueType vt = (TypeObjectWrapper.valueType) objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex;
        switch (vt)
        {
            case TypeObjectWrapper.valueType.Int:
                return objectWrapper.FindPropertyRelative("_int").intValue;

            case TypeObjectWrapper.valueType.Float:
                return objectWrapper.FindPropertyRelative("_float").floatValue;

            case TypeObjectWrapper.valueType.Bool:
                return objectWrapper.FindPropertyRelative("_bool").boolValue;

            case TypeObjectWrapper.valueType.UnityObject:
                return objectWrapper.FindPropertyRelative("_unityObject").objectReferenceValue;

            default:
                Debug.LogWarning("shit aint implemented yet");
                return null;
        }
    }

    void SetTypeObjectWrapperValue(SerializedProperty objectWrapper, System.Type pValueType)
    {
        TypeObjectWrapper.valueType vt = TypeObjectWrapper.getValueType(pValueType);
        switch (vt)
        {
            case TypeObjectWrapper.valueType.Int:
                objectWrapper.FindPropertyRelative("_int").intValue = 0;

                break;
            case TypeObjectWrapper.valueType.Float:
                objectWrapper.FindPropertyRelative("_float").floatValue = 0f;

                break;
            case TypeObjectWrapper.valueType.Bool:
                objectWrapper.FindPropertyRelative("_bool").boolValue = false;

                break;
            case TypeObjectWrapper.valueType.UnityObject:
                objectWrapper.FindPropertyRelative("_unityObject").objectReferenceValue = null;
                break;
            default:
                Debug.LogWarning("shit aint implemented yet");
                break;
        }

        objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex = (int) vt;
    }

    object GetDefaultValueForType(TypeObjectWrapper.valueType vt)
    {
        switch (vt)
        {
            case TypeObjectWrapper.valueType.Int:
                return 0;
            case TypeObjectWrapper.valueType.Float:
                return 0f;
            case TypeObjectWrapper.valueType.Bool:
                return false;
            case TypeObjectWrapper.valueType.UnityObject:
                return null;
            default:
                return null;
        }
    }

    void DisplayAwakePropertys(Rect position, SerializedProperty property, GUIContent label)
    {

    }

    void DisplayTriggerPropertys(Rect position, SerializedProperty property, GUIContent label)
    {
        NextLine(25);
        EditorGUI.PrefixLabel(CalculateRect(80, 15), new GUIContent("TriggerType:"));

        List<string> temp = StoryEventManager.GetTriggerTypes();
        byte _oldStringIndex = (byte) property.FindPropertyRelative("_interactionType").intValue;
        property.FindPropertyRelative("_interactionType").intValue = EditorGUI.Popup(CalculateRect(150, 0), _oldStringIndex, temp.ToArray());

        NextLine(35);

        EditorGUI.LabelField(CalculateRect(155, 50), new GUIContent("InteractionTriggerCount"));

        NextLine(25);
        EditorGUI.PrefixLabel(CalculateRect(30, 20), new GUIContent("Min:"));

        EditorGUI.PropertyField(CalculateRect(50, 0), property.FindPropertyRelative("_interactionCountBeforePlay"), GUIContent.none);
        EditorGUI.PrefixLabel(CalculateRect(30, 50), new GUIContent("Max:"));
        EditorGUI.PropertyField(CalculateRect(50, 10), property.FindPropertyRelative("_maxInteractionCount"), GUIContent.none);

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
            // CalculativeRect.x = GuiRect.x;
            //CalculativeRect.y += 20;
        }

        Rect TempRect = new Rect(CalculativeRect.x + PspaceWidth, CalculativeRect.y + PspaceHeight, Pwidth, Pheight);

        CalculativeRect = new Vector2(CalculativeRect.x + Pwidth + PspaceWidth, CalculativeRect.y + (pIgnoreHeightChange ? 0 : (Pheight + PspaceHeight)));

        return TempRect;
    }

    Rect CalculateRect(float Pwidth, float PspaceWidth, bool pIgnoreHeightChange = false)
    {
        if (CalculativeRect.x + PspaceWidth + Pwidth > GuiRect.width && !pIgnoreHeightChange)
        {
            // CalculativeRect.x = GuiRect.x - 5;
            // CalculativeRect.y += 20;
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
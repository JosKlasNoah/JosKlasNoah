using Custom.Story;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StoryEventContainer))]
public class GameEventPropteryDrawer : PropertyDrawer
{
    Dictionary<TriggerType, float> TriggerTypeHeight = new Dictionary<TriggerType, float>()
        {
            {TriggerType.Awake,30 },
            {TriggerType.TriggerEnter,180 },
           {TriggerType.TriggerExit,180 },
        {TriggerType.Interact, 140 }
        };

    string[] options = System.Enum.GetNames(typeof(TriggerType));

    Vector2 CalculativeRect;
    Rect GuiRect;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 70;

        TriggerType _tt = GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType"));
        if (TriggerTypeHeight.ContainsKey(_tt))
        {
            height += TriggerTypeHeight[_tt];
        }
        else
        {
            Debug.LogWarning("Unkown TriggerType:" + _tt + "'");
            height += 500;
        }

        if (_tt == TriggerType.TriggerEnter || _tt == TriggerType.TriggerExit)
        {
            int size = property.FindPropertyRelative("_eventRequiredmentList").arraySize;
            height += (size <= 0 ? 0 : 1) * 25 + 25 * size;
        }

        for (int i = 0; i < property.FindPropertyRelative("_storyEventsToPlay").arraySize; i++)
        {
            height += 15;
            SerializedProperty tempProp = property.FindPropertyRelative("_storyEventsToPlay").GetArrayElementAtIndex(i).FindPropertyRelative("_onStoryEventTriggerExecute");

            for (int ii = 0; ii < tempProp.arraySize; ii++)
            {
                height += 25;

                height += 25 * tempProp.GetArrayElementAtIndex(ii).FindPropertyRelative("_functionParameters").arraySize;
            }
        }

        height += property.FindPropertyRelative("_storyEventsToPlay").arraySize * 25;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GuiRect = position;
        CalculativeRect = new Vector2(position.x, position.y);
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(CalculateRect(position.width, 0, true), "", GUI.skin.horizontalSlider);

        NextLine(25);
        EditorGUI.PrefixLabel(CalculateRect(70, 0), new GUIContent("EventName:"));
        EditorGUI.PropertyField(CalculateRect(150, 10), property.FindPropertyRelative("_eventName"), GUIContent.none);

        NextLine(25);

        EditorGUI.PrefixLabel(CalculateRect(60, 15), new GUIContent("EventType:"));
        EditorGUI.PropertyField(CalculateRect(150, 20), property.FindPropertyRelative("_storyEventTriggerType"), GUIContent.none);

        switch (GetPropertyAsTriggerType(property.FindPropertyRelative("_storyEventTriggerType")))
        {
            case TriggerType.Awake:
                DisplayAwakePropertys(position, property, label);
                NextLine(25);
                break;

            case TriggerType.TriggerExit:
            case TriggerType.TriggerEnter:
                DisplayTriggerPropertys(position, property, label);
                NextLine(45);
                DrawEventInfo(property);
                NextLine(45);
                break;

            case TriggerType.Interact:
                DisplayInteractPropertys(position, property, label);
                NextLine(45);
                DrawEventInfo(property);
                NextLine(45);
                break;

            default:
                DisplayAllPropertys(position, property, label);
                Debug.LogWarning("TriggerType has no custom UI implemented");
                break;
        }



        EditorGUI.PrefixLabel(CalculateRect(115, 100), new GUIContent("StoryEventAmount:"), GUIStyle.none);

        SerializedProperty tempProp = property.FindPropertyRelative("_storyEventsToPlay");
        tempProp.arraySize = Mathf.Clamp(EditorGUI.IntField(CalculateRect(30, 0), tempProp.arraySize), 0, 50);
        for (int i = 0; i < tempProp.arraySize; i++)
        {
            NextLine(40);
            EditorGUI.PropertyField(CalculateRect(150, 0), tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_audioToPlay"), GUIContent.none, true);

            EditorGUI.PrefixLabel(CalculateRect(75, 0), new GUIContent("EventCount:"), GUIStyle.none);
            SerializedProperty _storyEvent = tempProp.GetArrayElementAtIndex(i).FindPropertyRelative("_onStoryEventTriggerExecute");
            _storyEvent.arraySize = Mathf.Clamp(EditorGUI.IntField(CalculateRect(30, 0), _storyEvent.arraySize), 0, 50);

            for (int y = 0; y < _storyEvent.arraySize; y++)
            {
                NextLine(25);
                DrawFunctionItem(_storyEvent.GetArrayElementAtIndex(y));
            }
        }

        EditorGUI.EndProperty();
    }

    void DrawEventInfo(SerializedProperty property)
    {
        // NextLine(20);
        EditorGUI.PrefixLabel(CalculateRect(125, 90), new GUIContent("RequiredEventCount:"), GUIStyle.none);
        property.FindPropertyRelative("_eventRequiredmentList").arraySize = Mathf.Clamp(EditorGUI.IntField(CalculateRect(30, 0), property.FindPropertyRelative("_eventRequiredmentList").arraySize), 0, 5);
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
    }

    void DrawFunctionItem(SerializedProperty property)
    {

        List<System.Reflection.MethodInfo> atemp = StoryDelegate.GetAllMethods();


        SerializedProperty tempBase = property;//.FindPropertyRelative("_storyDel");
                                               //  EditorGUI.LabelField(CalculateRect(150, 0), tempBase.FindPropertyRelative("_functionName").stringValue);

        int _oldStringIndex = StoryDelegate.MethodsToString(atemp).IndexOf(property.FindPropertyRelative("_functionName").stringValue);
        _oldStringIndex = (_oldStringIndex) < 0 ? 0 : _oldStringIndex;

        int _newStringIndex = EditorGUI.Popup(CalculateRect(GuiRect.width, 0), _oldStringIndex, StoryDelegate.MethodsToString(atemp, true).ToArray());

        if (_newStringIndex != _oldStringIndex || tempBase.FindPropertyRelative("_functionName").stringValue == "")
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
                DrawPropertyForCustomType(CalculateRect(300, 0), currentElement);
            }
        }
        else
        {
            tempBase = tempBase.FindPropertyRelative("_functionParameters");

            for (int i = 0; i < tempBase.arraySize; i++)
            {
                NextLine(25);
                SerializedProperty currentElement = tempBase.GetArrayElementAtIndex(i);
                DrawPropertyForCustomType(CalculateRect(300, 0), currentElement);
            }
        }
    }

    void DrawPropertyForCustomType(Rect trans, SerializedProperty objectWrapper)
    {
        TypeObjectWrapper.valueType vt = (TypeObjectWrapper.valueType) objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex;

        EditorGUI.PropertyField(trans, objectWrapper.FindPropertyRelative(TypeObjectWrapper._valueString[vt]), GUIContent.none);
    }

    object GetPropertyValueForCustomType(SerializedProperty objectWrapper)
    {
        TypeObjectWrapper.valueType vt = (TypeObjectWrapper.valueType) objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex;

        // objectWrapper.FindPropertyRelative(TypeObjectWrapper._valueObject[vt]);
        switch (vt)
        {
            case TypeObjectWrapper.valueType.Int:
                return objectWrapper.FindPropertyRelative("_int").intValue;

            case TypeObjectWrapper.valueType.Float:
                return objectWrapper.FindPropertyRelative("_float").floatValue;

            case TypeObjectWrapper.valueType.Bool:
                return objectWrapper.FindPropertyRelative("_bool").boolValue;

            case TypeObjectWrapper.valueType.Vector3:
                return objectWrapper.FindPropertyRelative("_vector3").vector3Value;

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
        objectWrapper.FindPropertyRelative("_currentValueType").enumValueIndex = (int) vt;
        //SerializedProperty prop = objectWrapper.FindPropertyRelative(TypeObjectWrapper._valueString[vt]);

        return;
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
            case TypeObjectWrapper.valueType.Vector3:
                return Vector3.zero;
            case TypeObjectWrapper.valueType.UnityObject:
                return null;
            default:
                return null;
        }
    }

    void DisplayAwakePropertys(Rect position, SerializedProperty property, GUIContent label)
    {

    }

    void DisplayInteractPropertys(Rect position, SerializedProperty property, GUIContent label)
    {
        NextLine(25);

        EditorGUI.LabelField(CalculateRect(155, 50), new GUIContent("InteractionTriggerCount"));

        NextLine(25);
        EditorGUI.PrefixLabel(CalculateRect(30, 20), new GUIContent("Min:"));

        EditorGUI.PropertyField(CalculateRect(50, 0), property.FindPropertyRelative("_interactionCountBeforePlay"), GUIContent.none);
        EditorGUI.PrefixLabel(CalculateRect(30, 50), new GUIContent("Max:"));
        EditorGUI.PropertyField(CalculateRect(50, 10), property.FindPropertyRelative("_maxInteractionCount"), GUIContent.none);
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "configFile")]
public class EditorConfig : ScriptableObject
{
    [SerializeField]
    List<string> _triggerTypes = new List<string>() { "PlayerController", "ObjectBase"};
    [SerializeField]
    List<string> _EventExecutionMehtods = new List<string>() { "PlayerHandler", "RepeatableEvents" };

    static List<Type> StringToType(List<string> pStringList)
    {
        List<Type> _TypeList = new List<Type>();

        foreach (string stringName in pStringList)
        {
            _TypeList.Add(Type.GetType(stringName));
        }

        return _TypeList;

    }

    public List<Type> TriggerTypes => StringToType(_triggerTypes);
    public List<Type> EventExecutionMethods => StringToType(_EventExecutionMehtods);

}

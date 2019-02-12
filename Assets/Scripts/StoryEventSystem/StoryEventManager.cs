using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Custom.Story
{
    using Custom.GameManager;

    public enum TriggerType
    {
        Awake,
        Trigger
    }

    [Serializable]
    public class StoryEventContainer
    {
        public StoryDelegate _storyDel = new StoryDelegate();

        public string _eventName = "DefaultEventName";
        public TriggerType _storyEventTriggerType = TriggerType.Awake;
        public byte _interactionCountBeforePlay = 1; // hoe vaak de trigger moet worden geactiaved om deze story event te triggeren
        public byte _maxInteractionCount = 1; // hoe vaak deze getriggerd kan worden
        public byte _currentInteractionCount = 0;
        public byte _interactionType;

        public List<StoryEventNameContainer> _eventRequiredmentList = new List<StoryEventNameContainer>();

        public List<StoryContainer> StoryEventsToPlay = new List<StoryContainer>();
    }

    [Serializable]
    public class StoryEventNameContainer
    {
        public string _eventName = "DefaultEventName";
        public bool _Completed = false;
    }

    [Serializable]
    public class StoryContainer
    {
        public AudioClip _audioToPlay;
        public UnityEvent _onStoryEventTriggerExecute = new UnityEvent();

        public void ExecuteStoryEvent()
        {


            if (_audioToPlay == null)
            {
                Debug.LogWarning("NoAudioSourceFound");
                StoryEventManager.OnAudioFinished();
                return;
            }

            GameManager.PlayAudio(_audioToPlay);
        }

        public void OnStoryPlayFinished()
        {
            _onStoryEventTriggerExecute.Invoke();
        }
    }

    [Serializable]
    public class StoryDelegate
    {
        public string _className = "";// we only care for editor purpose
        public string _functionName = "noEvent";
        public List<TypeObjectWrapper> _functionParameters = new List<TypeObjectWrapper>();

        static List<MethodInfo> _AllMethods;

        public void Invoke()
        {
            GetMethod(_functionName).Invoke(null, ContainerToRuntimeData());
        }

        object[] ContainerToRuntimeData()
        {
            object[] temp = new object[_functionParameters.Count];

            for (int i = 0; i < _functionParameters.Count; i++)
            {
                temp[i] = _functionParameters[i].GetValue();
            }

            return temp;

        }

        public StoryDelegate() { }

        public StoryDelegate(string pFunctionName, List<Type> pParameterType)
        {
            _functionName = pFunctionName;

            foreach (Type parType in pParameterType)
            {
                _functionParameters.Add(new TypeObjectWrapper(parType));
            }
        }

        public StoryDelegate(MethodInfo pMethod)
        {
            _functionName = pMethod.Name;

            foreach (Type parType in GetMethodParameterTypes(pMethod))
            {
                _functionParameters.Add(new TypeObjectWrapper(parType));
            }
        }

        public static List<string> MethodsToString(List<MethodInfo> pMethodInfo, bool addClassName = false)
        {
            List<string> temp = new List<string>();

            foreach (MethodInfo item in pMethodInfo)
            {
                temp.Add((addClassName ? item.ReflectedType + " : " : "") + item.Name);
            }
            return temp;
        }

        public static List<Type> GetMethodParemterTypes(Type pMethod)
        {
            List<Type> temp = new List<Type>();

            foreach (MethodInfo item in pMethod.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                foreach (var itemm in item.GetParameters())
                {

                    // itemm.ParameterType.//,0
                    object nigga = Activator.CreateInstance(Type.GetType(itemm.ParameterType.FullName));

                    Debug.Log(item.Name + ":" + nigga);
                    //tempa += itemm + ",";
                }


                //  Debug.Log(item + " : " + tempa);
            }
            return temp;
        }

        public static MethodInfo GetMethod(string name)
        {
            foreach (MethodInfo item in GetAllMethods())
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public static List<MethodInfo> GetAllMethods()
        {
            if (_AllMethods == null)
            {
                List<MethodInfo> AllMethods = new List<MethodInfo>();

                foreach (Type classType in StoryEventManager.GetEventExecutionMethods())
                {
                    foreach (MethodInfo items in classType.GetMethods(BindingFlags.Static | BindingFlags.Public))
                    {
                        AllMethods.Add(items);
                    }
                }
                _AllMethods = AllMethods;
            }

            return _AllMethods;
        }

        public static List<MethodInfo> GetAllMethods(Type pClass)
        {
            List<MethodInfo> AllMethods = new List<MethodInfo>();

            foreach (MethodInfo items in pClass.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                AllMethods.Add(items);
            }

            return AllMethods;
        }

        public static List<string> GetAllMethodsAsString()
        {
            return MethodsToString(GetAllMethods()) ;
        }

        public static Type[] GetMethodParameterTypes(MethodInfo _methodInfo)
        {
            List<Type> tempT = new List<Type>();
            foreach (var itemm in _methodInfo.GetParameters())
            {
                tempT.Add(Type.GetType(itemm.ParameterType.AssemblyQualifiedName + itemm.ParameterType.Name, true));
            }

            return tempT.ToArray();
        }

    }

    [System.Serializable]
    public class TypeObjectWrapper
    {
        public enum valueType
        {
            Int,
            Float,
            Bool,
            UnityObject
        }

        public int _int = 0;
        public float _float = 0;
        public bool _bool = false;
        public UnityEngine.Object _unityObject;
        public valueType _currentValueType;

        public TypeObjectWrapper(Type currentWrapperType)
        {
            SetValueType(currentWrapperType);
        }

        public valueType getValueType()
        {
            return _currentValueType;
        }

        public static valueType getValueType(Type pType)
        {
            if (pType == typeof(int))
            {
                return valueType.Int;
            }
            else if (pType == typeof(float))
            {
                return valueType.Float;
            }
            else if (pType == typeof(bool))
            {
                return valueType.Bool;
            }
            else if (pType == typeof(GameObject))
            {
                return valueType.UnityObject;
            }

            Debug.LogWarning("not implemented type:" + pType);
            return valueType.Bool;
        }

        public void SetValueType(object pObject)
        {
            SetValueType(pObject.GetType());
        }

        public void SetValueType(Type pType)
        {
            _currentValueType = getValueType(pType);
        }

        public void SetValue(object pValue)
        {

            valueType valueType = getValueType(pValue.GetType());
            switch (valueType)
            {
                case valueType.Int:
                    _int = (int) pValue;
                    break;
                case valueType.Float:
                    _float = (float) pValue;
                    break;
                case valueType.Bool:
                    _bool = (bool) pValue;
                    break;
                case valueType.UnityObject:
                    _unityObject = (UnityEngine.Object) pValue;
                    break;
                default:
                    Debug.LogWarning("not implemented type");
                    break;
            }
            _currentValueType = valueType;
        }

        public object GetValue()
        {
            switch (_currentValueType)
            {
                case valueType.Int:
                    return _int;
                case valueType.Float:
                    return _float;
                case valueType.Bool:
                    return _bool;
                case valueType.UnityObject:
                    return _unityObject;
                default:
                    Debug.LogWarning("not implemented type");
                    return null;
            }
        }
    }

    public class StoryEventManager
    {
        static readonly List<Type> _triggerTypes = new List<Type>() { typeof(PlayerController), typeof(TestObjectInteraction) };
        static readonly List<Type> _EventExecutionMehtods = new List<Type>() { typeof(PlayerHandler), typeof(RepeatableEvents) };
        static readonly List<TriggerType> _requiresColliders = new List<TriggerType>() { TriggerType.Trigger };

        static StoryEventManager instance;
        Queue<StoryContainer> _storyEventQue = new Queue<StoryContainer>();

        public StoryEventManager()
        {
            instance = this;
        }

        public static bool RequiresCollider(StoryEventContainer PstoryEvent)
        {
            return _requiresColliders.Contains(PstoryEvent._storyEventTriggerType);
        }

        public static bool RequiresCollider(List<StoryEventContainer> PstoryEvents)
        {
            foreach (StoryEventContainer storyEventContainer in PstoryEvents)
            {
                if (_requiresColliders.Contains(storyEventContainer._storyEventTriggerType))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<string> GetTriggerTypes()
        {
            List<string> temp = new List<string>();

            for (int i = 0; i < _triggerTypes.Count; i++)
            {

                temp.Add(_triggerTypes[i].Name);
            }

            return temp;
        }

        public static string GetTypeAsString(int index)
        {
            return _triggerTypes[index].Name;
        }


        public static List<string> GetEventExecutionMethodsAsString()
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < _triggerTypes.Count; i++)
            {

                temp.Add(_EventExecutionMehtods[i].Name);
            }

            return temp;
        }
        public static string GetEventExecutionMethodAsString(int index)
        {
            return _EventExecutionMehtods[index].ToString();
        }
        public static List<Type> GetEventExecutionMethods()
        {
            return _EventExecutionMehtods;
        }


        public static void QueStoryEvents(List<StoryContainer> newEvents)
        {

            if (newEvents.Count <= 0)
            {
                Debug.LogWarning("new event que is empty");
                return;
            }

            if (instance._storyEventQue.Count > 0)
            {
                instance._storyEventQue.Clear();
            }

            foreach (StoryContainer pStoryEvent in newEvents)
            {
                instance._storyEventQue.Enqueue(pStoryEvent);
            }
            instance._storyEventQue.Peek().ExecuteStoryEvent();
        }

        public static void OnAudioFinished()
        {
            instance._storyEventQue.Dequeue().OnStoryPlayFinished();
            PlayNext();
        }

        static void PlayNext()
        {
            if (instance._storyEventQue.Count != 0)
            {
                instance._storyEventQue.Peek().ExecuteStoryEvent();
            }
        }
    }
}

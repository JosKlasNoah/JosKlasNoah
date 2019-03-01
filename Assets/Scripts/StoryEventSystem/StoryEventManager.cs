using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Custom.Story
{

    #region Containers
    using Custom.GameManager;
    using System.Linq;

    public enum TriggerType
    {
        Awake,
        TriggerEnter,
        TriggerExit,
        Interact
    }

    [Serializable]
    public class StoryEventContainer
    {
        //public StoryDelegate _storyDel = new StoryDelegate();

        public string _eventName = "DefaultEventName";
        public TriggerType _storyEventTriggerType = TriggerType.Awake;
        public byte _interactionCountBeforePlay = 1; // hoe vaak de trigger moet worden geactiaved om deze story event te triggeren
        public byte _maxInteractionCount = 1; // hoe vaak deze getriggerd kan worden
        public byte _currentInteractionCount = 0;
        public byte _interactionType;

        public List<StoryEventNameContainer> _eventRequiredmentList = new List<StoryEventNameContainer>();

        public List<StoryContainer> _storyEventsToPlay = new List<StoryContainer>();

        public bool CanExecuteStoryEvent(GameObject obj = null)
        {
            if (_storyEventTriggerType == TriggerType.Awake)
            {
                return true;
            }

            if (_storyEventTriggerType == TriggerType.TriggerEnter || _storyEventTriggerType == TriggerType.TriggerExit)
            {
                if (obj == null)
                {
                    return false;
                }

                if (obj.GetComponent(StoryEventManager.GetTypeAsString(_interactionType)) == null)
                {
                    return false;
                }
            }


            {
                bool HasCompletedAll = true;
                foreach (StoryEventNameContainer _storyName in _eventRequiredmentList)
                {
                    if (_storyName._Completed != StoryEventManager.HasStoryEventCompleted(_storyName._eventName))
                    {
                        HasCompletedAll = false;
                        break;
                    }
                }

                if (!HasCompletedAll)
                {
                    Debug.Log("has not completed all required events");
                    return false;
                }
            }

            if (_currentInteractionCount >= _interactionCountBeforePlay + _maxInteractionCount)
            {
                return false;

            }

            _currentInteractionCount++;

            if (_interactionCountBeforePlay >= _currentInteractionCount)
            {
                Debug.Log("B");
                return false;
            }


            Debug.Log("Exited the loop");
            return true;
        }

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
        public List<StoryDelegate> _onStoryEventTriggerExecute = new List<StoryDelegate>();

        public void ExecuteStoryEvent()
        {
            if (_audioToPlay == null)
            {
                Debug.Log("NoAudioSourceFound");
                StoryEventManager.OnAudioFinished();
                return;
            }

            GameManager.PlayAudio(_audioToPlay);
        }

        public void OnStoryPlayFinished()
        {
            foreach (StoryDelegate sd in _onStoryEventTriggerExecute)
            {
                sd.Invoke();
            }
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

            try
            {
                GetMethod(_functionName).Invoke(null, ContainerToRuntimeData());
            }
            catch (Exception e)
            {
                Debug.LogError(_functionName + " is a unkown function");
                Debug.LogError(e);
            }
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
                temp.Add((addClassName ? item.ReflectedType + "/" : "") + item.Name);
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

            List<MethodInfo> AllMethods = new List<MethodInfo>();

            foreach (Type classType in StoryEventManager.EventExecutionMethods)
            {
                foreach (MethodInfo items in classType.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    AllMethods.Add(items);
                }
            }
            _AllMethods = AllMethods;


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
            return MethodsToString(GetAllMethods());
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
        public static Dictionary<valueType, string> _valueString = new Dictionary<valueType, string>()
        {
            { valueType.Int, "_int" },
            { valueType.Float, "_float"},
            {valueType.Bool,"_bool" },
            {valueType.Vector3,"_vector3" },
            {valueType.UnityObject ,"_unityObject" }
        };

        static Dictionary<Type, valueType> _valueType = new Dictionary<Type, valueType>()
        {
            {typeof(int),valueType.Int },
            {typeof(float),valueType.Float },
            {typeof(bool),valueType.Bool },
            {typeof(Vector3),valueType.Vector3 },
            {typeof(GameObject),valueType.UnityObject }
        };

        public enum valueType
        {
            Int,
            Float,
            Bool,
            Vector3,
            UnityObject
        }

        public int _int = 0;
        public float _float = 0;
        public bool _bool = false;
        public UnityEngine.Object _unityObject;
        public Vector3 _vector3;

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
            return _valueType[pType];
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
                case valueType.Vector3:
                    return _vector3;
                case valueType.UnityObject:
                    return _unityObject;
                default:
                    Debug.LogWarning("not implemented type");
                    return null;
            }
        }
    }
    #endregion

    public class StoryEventManager
    {
        static List<Type> _triggerTypesWithChildClasses;
        static readonly List<TriggerType> _requiresColliders = new List<TriggerType>() { TriggerType.TriggerEnter, TriggerType.TriggerExit, TriggerType.Interact };
        List<string> _completedStoryEvents = new List<string>();

        EditorConfig _config;

        static StoryEventManager instance;

        Queue<StoryContainer> _storyEventQue = new Queue<StoryContainer>();

        public static List<Type> EventExecutionMethods
        {
            get
            {
                if (instance == null)
                {
                    return ((EditorConfig) Resources.Load("EditorConfig")).EventExecutionMethods;
                }

                return instance._config.EventExecutionMethods;
            }
        }

        public static List<Type> TriggerTypes
        {
            get
            {
                if (instance == null)
                {
                    return ((EditorConfig) Resources.Load("EditorConfig")).TriggerTypes;
                }

                return instance._config.TriggerTypes;
            }
        }

        public StoryEventManager()
        {
            _config = (EditorConfig) Resources.Load("EditorConfig");
            instance = this;
        }

        void FinishStoryEvent(string name)
        {
            if (!_completedStoryEvents.Contains(name))
            {
                _completedStoryEvents.Add(name);
            }
        }

        public static void FinishQue()
        {
            if (instance._storyEventQue.Count <= 0)
            {
                return;
            }

            do
            {
                instance._storyEventQue.Dequeue().OnStoryPlayFinished();
            }
            while (instance._storyEventQue.Count > 0);

            GameManager.PauseAudio();
        }

        public static bool HasStoryEventCompleted(string name)
        {
            return instance._completedStoryEvents.Contains(name);
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

            foreach (var triggerType in TriggerTypes)
            {
                List<string> _tempStringList = GetChildStringList(triggerType, triggerType.Name);

                foreach (var item in _tempStringList)
                {
                    temp.Add(item);
                }

            }

            return temp;
        }

        static List<string> GetChildStringList(Type pCType, string AppendString = "")
        {
            List<string> temp = new List<string>();

            Type[] tempTypeArray = GetChildClasses(pCType, true);

            if (tempTypeArray.Length > 0)
            {
                temp.Add(AppendString + "/All");

                foreach (Type childClass in tempTypeArray)
                {
                    List<string> _childStringList = GetChildStringList(childClass, childClass.Name);

                    foreach (string item in _childStringList)
                    {
                        temp.Add(AppendString + "/" + item);
                    }
                }
            }
            else
            {
                temp.Add(pCType.Name);
            }
            return temp;
        }

        public static Type[] GetChildClasses(Type pT, bool pDirectChildOnly = false)
        {
            if (_triggerTypesWithChildClasses == null)
            {
                CreateTriggerTypesWithChildClasses();
            }
            if (pDirectChildOnly)
            {
                return _triggerTypesWithChildClasses.Where(t => t.BaseType == pT).ToArray();
            }
            else
            {
                return _triggerTypesWithChildClasses.Where(t => t.IsSubclassOf(pT)).ToArray();

            }
        }

        static void CreateTriggerTypesWithChildClasses()
        {
            _triggerTypesWithChildClasses = new List<Type>();
            foreach (var triggerType in TriggerTypes)
            {
                Type[] childClasses = Assembly.GetAssembly(triggerType).GetTypes().Where(t => t.IsSubclassOf(triggerType)).ToArray();

                _triggerTypesWithChildClasses.Add(triggerType);

                foreach (Type ChildTriggerType in childClasses)
                {
                    _triggerTypesWithChildClasses.Add(ChildTriggerType);
                }
            }
        }

        public static string GetTypeAsString(int index)
        {
            if (_triggerTypesWithChildClasses == null)
            {
                CreateTriggerTypesWithChildClasses();
            }

            return _triggerTypesWithChildClasses[index].Name;
        }

        public static List<string> GetEventExecutionMethodsAsString()
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < TriggerTypes.Count; i++)
            {

                temp.Add(EventExecutionMethods[i].Name);
            }

            return temp;
        }

        public static string GetEventExecutionMethodAsString(int index)
        {
            return EventExecutionMethods[index].ToString();
        }

        public static void QueStoryEvents(List<StoryContainer> newEvents, string EventName)
        {

            instance.FinishStoryEvent(EventName);

            if (newEvents.Count <= 0)
            {
                Debug.LogWarning("new event que is empty");
                return;
            }

            if (instance._storyEventQue.Count > 0)
            {
                do
                {
                    instance._storyEventQue.Dequeue().OnStoryPlayFinished();
                }
                while (instance._storyEventQue.Count > 0);
            }

            foreach (StoryContainer pStoryEvent in newEvents)
            {
                instance._storyEventQue.Enqueue(pStoryEvent);
            }

            instance._storyEventQue.Peek().ExecuteStoryEvent();
        }

        public static void OnAudioFinished()
        {
            if (instance._storyEventQue.Count > 0)
            {
                instance._storyEventQue.Dequeue().OnStoryPlayFinished();

                PlayNext();
            }
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

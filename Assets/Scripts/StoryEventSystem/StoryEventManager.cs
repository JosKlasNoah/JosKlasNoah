using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Custom.Story
{
    using Custom.GameManager;

    public enum TriggerType
    {
        Awake,
        Trigger
    }

    [System.Serializable]
    public class StoryEventNameContainer
    {
        public string _eventName = "DefaultEventName";
        public bool _Completed = false;
    }

    [System.Serializable]
    public class StoryEventContainer
    {
        [SerializeField]
        public string _eventName = "DefaultEventName";
        public TriggerType _storyEventTriggerType = TriggerType.Awake;
        public int _interactionCountBeforePlay = 1; // hoe vaak de trigger moet worden geactiaved om deze story event te triggeren
        public int _maxInteractionCount = 1; // hoe vaak deze getriggerd kan worden
        public int _currentInteractionCount = 0;
        public string _interactionType;

        public List<StoryEventNameContainer> _eventRequiredmentList = new List<StoryEventNameContainer>();

        public List<StoryContainer> StoryEventsToPlay = new List<StoryContainer>();
    }

    [System.Serializable]
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


    public class StoryEventManager
    {
        static readonly List<Type> _triggerTypes = new List<Type>() { typeof(PlayerController), typeof(TestObjectInteraction) };
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

        public static Type GetTypeFromString(int index)
        {
            return _triggerTypes[index];
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

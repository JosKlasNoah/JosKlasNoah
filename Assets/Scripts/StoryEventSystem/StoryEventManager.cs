using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Custom.Story
{
    using Custom.GameManager;

    public enum TriggerType
    {
        Awake,
        Player
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

        public List<StoryContainer> StoryEventsToPlay = new List<StoryContainer>();
    }

    [System.Serializable]
    public class StoryContainer
    {
        public string t = "";
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
        static readonly List<TriggerType> _requiresColliders = new List<TriggerType>() { TriggerType.Player };

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

        public static void QueStoryEvents(List<StoryContainer> newEvents)
        {

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

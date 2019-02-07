using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.StoryEventManager;
using UnityEngine.Events;

public class StoryEventOnAwake : MonoBehaviour
{

    [SerializeField]
    List<StoryContainer> _storyChainEvents = new List<StoryContainer>();

    private void OnValidate()
    {
        for (int i = 0; i < _storyChainEvents.Count; i++)
        {
            if (_storyChainEvents[i].storyEventToExecute != null)
                _storyChainEvents[i].storyEventToExecute.AddStoryListeners(_storyChainEvents[i].OnStoryEventTriggerExecute);
        }

    }

    private void Awake()
    {
        StoryEventManager.QueStoryEvents(StoryContainer.GetStorysToExecute(_storyChainEvents));
    }
}

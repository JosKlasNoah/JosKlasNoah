using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.StoryEventManager;
using UnityEngine.Events;

enum TriggerType
{
    Player
}

[System.Serializable]
class StoryContainer
{
    public StoryEvent_SO storyEventToExecute;
    public UnityEvent OnStoryEventTriggerExecute = new UnityEvent();

    public static List<StoryEvent_SO> GetStorysToExecute( List<StoryContainer> data)
    {
        List<StoryEvent_SO> temp = new List<StoryEvent_SO>();

        foreach (StoryContainer sc in data)
        {
            temp.Add(sc.storyEventToExecute);
        }

        return temp;
    }
}

[RequireComponent(typeof(BoxCollider))]
public class StoryEventTrigger : MonoBehaviour
{
   
    [SerializeField]
    TriggerType _storyEventTriggerType;

    List<StoryContainer> _storyChainEvents = new List<StoryContainer>();
    

    BoxCollider _collider;

    private void OnValidate()
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }
        _collider.isTrigger = true;

        for (int i = 0; i < _storyChainEvents.Count; i++)
        {
            if (_storyChainEvents[i].storyEventToExecute != null)
                _storyChainEvents[i].storyEventToExecute.AddStoryListeners(_storyChainEvents[i].OnStoryEventTriggerExecute);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_storyEventTriggerType)
        {
            case TriggerType.Player:
                if (other.gameObject.GetComponent<PlayerController>() != null)
                    StoryEventManager.ExecuteStoryEvent(StoryContainer.GetStorysToExecute(_storyChainEvents));
                break;
            default:
                Debug.LogError(_storyEventTriggerType + " has not been a story event trigger thats implemented");
                break;
        }
    }
}

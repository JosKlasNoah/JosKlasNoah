using Custom.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent()]
public class StoryEvent : MonoBehaviour
{
    [SerializeField]
    public List<StoryEventContainer> _storyEvents = new List<StoryEventContainer>();

    BoxCollider _collider;


    #region Editor
    private void OnValidate()
    {

        foreach (StoryEventContainer storyEventContainer in _storyEvents)
        {
            if (storyEventContainer._eventName == "")
            {
                storyEventContainer._eventName = "Default Name";
            }

            if (0 >= storyEventContainer._maxInteractionCount)
            {
                storyEventContainer._maxInteractionCount++;
            }
        }

        if (StoryEventManager.RequiresCollider(_storyEvents))
        {
            _collider = GetComponent<BoxCollider>();
            if (_collider == null)
            {
                StartCoroutine(FixColiderStatus(true));
                return;
            }

            _collider.isTrigger = true;
        }
        else
        {
            if (_collider != null)
            {
                StartCoroutine(FixColiderStatus(false));
            }
        }
    }
    //we are not allowed to destory the component in OnValidate this is the workaround (wait till the end of the frame
    IEnumerator FixColiderStatus(bool addCollider)
    {
        yield return new WaitForEndOfFrame();
        if (addCollider)
        {
            gameObject.AddComponent<BoxCollider>();
            _collider = GetComponent<BoxCollider>();

            _collider.isTrigger = true;
        }
        else
        {
            DestroyImmediate(GetComponent<BoxCollider>());
        }
    }
    #endregion;


    private void Awake()
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent.CanExecuteStoryEvent())
            {
                StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {

            if (PstoryEvent.CanExecuteStoryEvent(other.gameObject) && PstoryEvent._storyEventTriggerType == TriggerType.Trigger)
            {
                StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                return;
            }
        }

    }
}


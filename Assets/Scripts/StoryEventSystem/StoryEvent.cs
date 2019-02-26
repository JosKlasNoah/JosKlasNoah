using Custom.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

[DisallowMultipleComponent()]
public class StoryEvent : MonoBehaviour
{
    [SerializeField]
    public List<StoryEventContainer> _storyEvents = new List<StoryEventContainer>();

    BoxCollider _collider;


    #region Editor

#if UNITY_EDITOR


    private void OnValidate()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
                if (gameObject.activeSelf)
                    StartCoroutine(FixColiderStatus(true));
                return;
            }

            _collider.isTrigger = true;
        }
        else
        {
            if (_collider != null)
            {
                if (gameObject.activeSelf)
                    StartCoroutine(FixColiderStatus(false));
            }
        }
    }
    //we are not allowed to destory the component in OnValidate this is the workaround (wait till the end of the frame
    IEnumerator FixColiderStatus(bool addCollider)
    {
        yield return new EditorWaitForSeconds(.1f);

        if (addCollider)
        {
            if (_collider == null)
            {
                gameObject.AddComponent<BoxCollider>();
                _collider = GetComponent<BoxCollider>();

                _collider.isTrigger = true;
            }
        }
        else
        {

            DestroyImmediate(_collider);
        }
    }

#endif
    #endregion;


    private void Awake()
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent._storyEventTriggerType == TriggerType.Awake)
            {
                if (PstoryEvent.CanExecuteStoryEvent())
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent._storyEventTriggerType == TriggerType.TriggerEnter)
                if (PstoryEvent.CanExecuteStoryEvent(other.gameObject))
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                    return;
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent._storyEventTriggerType == TriggerType.TriggerExit)
                if (PstoryEvent.CanExecuteStoryEvent(other.gameObject))
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                    return;
                }
        }
    }

}


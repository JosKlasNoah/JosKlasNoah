using Custom.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

[DisallowMultipleComponent()]
public class StoryEvent : MonoBehaviour, IInteractable
{
    [SerializeField]
    public List<StoryEventContainer> _storyEvents = new List<StoryEventContainer>();

    Collider _collider;


    #region Editor

#if UNITY_EDITOR


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
            _collider = GetComponent<Collider>();
            if (_collider == null)
            {
                if (gameObject.activeSelf)
                {
                    StartCoroutine(FixColiderStatus(true, false));
                }

                return;
            }

            foreach (StoryEventContainer storyEventContainer in _storyEvents)
            {
                if (storyEventContainer._storyEventTriggerType == TriggerType.Interact)
                {
                    if (!GetComponent<Rigidbody>())
                    {
                        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                    }

                }
            }
            gameObject.layer = 0;

        }
        else
        {
            if (_collider != null)
            {
                if (gameObject.activeSelf)
                {
                    StartCoroutine(FixColiderStatus(false));
                }
            }

            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    //we are not allowed to destroy the component in OnValidate. This is the workaround (wait till the end of the frame)
    IEnumerator FixColiderStatus( bool addCollider, bool trigger = true ) {
        yield return new EditorWaitForSeconds(.05f);

        if (addCollider) {
            if (_collider == null) {
                gameObject.AddComponent<BoxCollider>();
                _collider = GetComponent<BoxCollider>();

                _collider.isTrigger = trigger;
            }
        }
        else {

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
            {
                if (PstoryEvent.CanExecuteStoryEvent(other.gameObject))
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent._storyEventTriggerType == TriggerType.TriggerExit)
            {
                if (PstoryEvent.CanExecuteStoryEvent(other.gameObject))
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                    return;
                }
            }
        }
    }

    public void OnItemInteract(PlayerController owningPlayer) {
        Debug.Log("interact");

        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (PstoryEvent._storyEventTriggerType == TriggerType.Interact)
            {
                if (PstoryEvent.CanExecuteStoryEvent())
                {
                    StoryEventManager.QueStoryEvents(PstoryEvent._storyEventsToPlay, PstoryEvent._eventName);
                    return;
                }
            }
        }
    }

    public void OnItemRightMouseButton(PlayerController owningPlayer) {
        throw new System.NotImplementedException();
    }

    public void UpdateObjectOffset(float newPosistion) {
        throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }
}


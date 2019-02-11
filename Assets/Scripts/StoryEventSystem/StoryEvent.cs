using Custom.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent()]
public class StoryEvent : MonoBehaviour
{
    [SerializeField]
    List<StoryEventContainer> _storyEvents = new List<StoryEventContainer>();

    BoxCollider _collider;

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

    private void Awake()
    {
        StoryEventManager.GetAllExectionMethods();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (StoryEventContainer PstoryEvent in _storyEvents)
        {
            if (!StoryEventManager.RequiresCollider(PstoryEvent))
            {
                continue;
            }

            if (PstoryEvent._currentInteractionCount >= PstoryEvent._maxInteractionCount)
            {
                continue;
            }

            PstoryEvent._currentInteractionCount++;

            if (PstoryEvent._interactionCountBeforePlay >= PstoryEvent._currentInteractionCount)
            {
                continue;
            }

            if (other.gameObject.GetComponent(StoryEventManager.GetTypeAsString(PstoryEvent._interactionType)) != null)
            {
                ///TODO REIMPLEMENT EVERYTHING
           //     StoryEventManager.QueStoryEvents(_storyEvents);
            }
        }

    }
}


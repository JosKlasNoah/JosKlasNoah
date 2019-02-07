using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Custom.GameManager;

public class UnityStoryEvent : UnityEvent<StoryEvent_SO>
{

}

[CreateAssetMenu(fileName = "StoryEvent", menuName = "Custom")]
public class StoryEvent_SO : ScriptableObject
{
    UnityEvent onStoryPlay = new UnityEvent();
    [SerializeField]
    AudioClip _audioToPlay;

    public void AddStoryListeners(UnityEvent actions)
    {
        onStoryPlay = actions;
    }

    public void ExecuteStoryEvent()
    {

    }
}

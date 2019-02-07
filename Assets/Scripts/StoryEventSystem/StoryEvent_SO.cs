using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Custom.GameManager;
using Custom.StoryEventManager;

[CreateAssetMenu(fileName = "StoryEvent", menuName = "Custom")]
public class StoryEvent_SO : ScriptableObject
{
    UnityEvent _onStoryPlay = new UnityEvent();
    [SerializeField]
    AudioClip _audioToPlay;

    public void AddStoryListeners(UnityEvent actions)
    {
        _onStoryPlay = actions;
    }

    public void ExecuteStoryEvent()
    {
        if(_audioToPlay == null)
        {
            Debug.LogWarning("NoAudioSourceFound");
            StoryEventManager.OnAudioFinished();
            return;
        }

        GameManager.PlayAudio(_audioToPlay);
    }

    public void PlayOnStoryPlay()
    {
        _onStoryPlay.Invoke();
    }
}

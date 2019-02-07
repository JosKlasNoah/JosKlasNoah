using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Custom.GameManager
{
    using Custom.StoryEventManager;

    public class GameManager
    {
        static GameManager instance;

        StoryEventManager StoryManager;
        AudioSource _currentAudioSource;

        public static AudioSource CurrentAudioSource { get { return instance._currentAudioSource; } set { instance._currentAudioSource = value; } }

        GameManager()
        {
            if (instance == null)
            {
                StoryManager = new StoryEventManager();
                instance = this;
                Debug.Log("test");
            }
        }


        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            if (instance == null)
            {
                new GameManager();
            }
        }

        static void PlayAudio(AudioClip PAudioClip)
        {
            instance._currentAudioSource.clip = PAudioClip;
            instance._currentAudioSource.Play();
        }
    }
}
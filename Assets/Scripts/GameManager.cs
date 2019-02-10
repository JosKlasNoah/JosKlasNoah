using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Custom.GameManager
{
    using Custom.Story;

    public class GameManager
    {
        static GameManager instance;

        StoryEventManager StoryManager;
        AudioSource _currentAudioSource;
        PlayerController _player;

        public static PlayerController CurrentPlayerController
        {
            get { return instance._player; }
            set
            {
                if (instance == null)
                    new GameManager();

                instance._player = value;
            }
        }

        public static AudioSource CurrentAudioSource
        {
            get { return instance._currentAudioSource; }
            set
            {
                if (instance == null)
                    new GameManager();

                instance._currentAudioSource = value;
            }
        }

        GameManager()
        {
            if (instance == null)
            {
                StoryManager = new StoryEventManager();
                instance = this;
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

        public static void PlayAudio(AudioClip PAudioClip)
        {
            instance._currentAudioSource.clip = PAudioClip;
            instance._currentAudioSource.Play();
        }
    }
}
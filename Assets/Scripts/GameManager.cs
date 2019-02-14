using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Custom.GameManager
{
    using Custom.Story;

    [SerializeField]
    class SaveDataContainer
    {
        public float mouseVelocity = 3;
    }

    public class GameManager
    {
        static GameManager instance;

        StoryEventManager StoryManager;
        AudioSource _currentAudioSource;
        PlayerController _player;
        SaveDataContainer _saveData = new SaveDataContainer();

        public static PlayerController CurrentPlayerController
        {
            get => instance._player;
            set
            {
                if (instance == null)
                    new GameManager();

                instance._player = value;
            }
        }

        public static AudioSource CurrentAudioSource
        {
            get => instance._currentAudioSource;
            set
            {
                if (instance == null)
                    new GameManager();

                instance._currentAudioSource = value;
            }
        }

        public static float MouseVelocity
        {
            get => instance._saveData.mouseVelocity;
            set
            {
                if (instance == null)
                    new GameManager();

                instance._saveData.mouseVelocity = value;
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
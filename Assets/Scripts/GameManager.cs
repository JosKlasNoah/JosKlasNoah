using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom.GameManager
{
    using Custom.Story;

    [SerializeField]
    class SaveDataContainer
    {
        public float mouseVelocity = 8f;
    }

    public class GameManager
    {
        static GameManager instance;

        StoryEventManager StoryManager;
        AudioSource _currentAudioSource;
        PlayerController _player;
        SaveDataContainer _saveData = new SaveDataContainer();

        float[] _objecInteractDistance = new float[] { 1, 2f };

        public static float[] objectInteractDistance => instance._objecInteractDistance;

        public static PlayerController CurrentPlayerController
        {
            get => instance._player;
            set
            {
                if (instance == null)
                    new GameManager();

                instance._player = value;
#if UNITY_EDITOR

                MouseVelocity = value.PlayerDataSO.mouseSpeed;
#endif
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
            get => instance._saveData.mouseVelocity * 10;
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
                Application.targetFrameRate = -1;

#if UNITY_STANDALONE
                  //StartGame();
#endif
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


        public static void StartGame()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);  
        }

        public static void PlayAudio(AudioClip PAudioClip)
        {
            instance._currentAudioSource.clip = PAudioClip;
            instance._currentAudioSource.Play();
        }

        public static void PauseAudio()
        {
            instance._currentAudioSource.Pause();
        }
    }
}
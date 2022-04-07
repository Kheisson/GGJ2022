using System;
using System.Collections.Generic;
using DG.Tweening;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private DataManager dataManager;
        [SerializeField] private SceneLoadHandler sceneLoadHandler;

        private static GameManager _instance;

        private static bool _isAudioMuted = false;

        private static HashSet<int> _clearedLevels;
        #endregion

        #region Events

        public Action<int, string> CreditUIEvent;

        #endregion

        #region Properties

        public static GameManager Instance => _instance;
        public static bool IsAudioMuted => _isAudioMuted;

        #endregion

        #region Methods

        private void Awake()
        {
            Init();
            StartLevel();
        }

        /// <summary>
        /// Instantiates a instance of the level and subscribes to the
        /// player and level manager events 
        /// </summary>
        private void StartLevel()
        {
            SubscribeToLevelEvents();
            _clearedLevels = DataManager.GetClearedLevels();
        }

        /// <summary>
        /// Instantiates a data manager for saving capabilities and loads the GameObject to DontDestroy
        /// </summary>
        private void Init()
        {
            if (Instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(gameObject);

            dataManager = Instantiate(dataManager, transform);
        }

        /// <summary>
        /// Subscribes to UI scene being loaded, to player moved to starting point
        /// Finished spawning event and credit wallet event
        /// </summary>
        private void SubscribeToLevelEvents()
        {
            CreditUIEvent += OnCreditUIEvent;
        }

        /// <summary>
        /// Event is called for pickups to credit the wallet when pickup picked 
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="pickupName"></param>
        private void OnCreditUIEvent(int credit, string pickupName)
        {
            //Only for signature purposes, used through instance in the UI/Enemy scripts
        }
        
        //Mutes the Audio source on the Main Camera
        public static bool Mute()
        {
            _isAudioMuted = !_isAudioMuted;
            var audioSource = Camera.main.GetComponent<AudioSource>();
            audioSource.mute = _isAudioMuted;
            return _isAudioMuted;
        }

        /// <summary>
        /// Unloads the UI scene, and reloads the active scene
        /// </summary>
        public void ReloadLevel()
        {
            //TODO: Make a scene manager
            DOTween.KillAll();

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("main")).completed += operation =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("Reloading Scene");
                Time.timeScale = 1f;
            };
        }

        //Loads the LoadingScene scene
        public void LoadLevel(string level)
        {
            SceneManager.LoadSceneAsync("LoadingScene").completed += operation =>
            {
                var levelLoader = FindObjectOfType<SceneLoadHandler>();
                levelLoader.LoadLevel(level);
            };
        }

        //Loads the Map scene
        public void LoadMap()
        {
            SceneManager.LoadSceneAsync("LoadingScene").completed += operation =>
            {
                var levelLoader = FindObjectOfType<SceneLoadHandler>();
                levelLoader.LoadToMap();
            };
        }

        public void AddCompletedLevel(string level)
        {
            _clearedLevels.Add(int.Parse(level.Split('l')[1]));
        }

        public bool GetClearedLevel(int level)
        {
            return _clearedLevels.Contains(level);
        }

        #endregion
    }
}
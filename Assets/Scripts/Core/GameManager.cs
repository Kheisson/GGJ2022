using System;
using DG.Tweening;
using Level;
using Player;
using Save;
using Spawn;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private LevelManager levelManager;
        [SerializeField] private DataManager dataManager;
        [SerializeField] private Popup failLevelPopup;

        private static GameManager _instance;
        private PlayerControl _playerInstance;
        private LevelManager _levelManager;

        private static bool _isAudioMuted = false;

        #endregion

        #region Events

        public Action StartLevelEvent;
        public Action EndLevelEvent;
        public Action<int, string> CreditUIEvent;

        #endregion

        #region Properties

        public static GameManager Instance => _instance;
        public static bool IsAudioMuted => _isAudioMuted;

        #endregion

        #region Methods

        private void Start()
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
            _levelManager = Instantiate(levelManager, transform);
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            _playerInstance = playerGameObject.GetComponent<PlayerControl>();
            SubscribeToLevelEvents();
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
            _levelManager.UISceneLoadedEvent += OnUISceneLoadedEvent;
            _playerInstance.PlayerMovedToStartingPosition += OnStartLevelEvent;
            _playerInstance.PlayerDefeatedEvent += OnPlayerDefeatedEvent;
            SpawnManager.Instance.FinishedSpawningEvent += OnFinishedSpawningEvent;
            CreditUIEvent += OnCreditUIEvent;
        }

        private void OnUISceneLoadedEvent()
        {
            Debug.Log("UI scene <color=green>loaded</color>");
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
        
        private void OnFinishedSpawningEvent()
        {
            var uiManager = GameObject.FindGameObjectWithTag("UIManager");
            var playerBalanceOnLevelFinished = uiManager.GetComponent<PlayerStatsUI>().CoinBalance;
            DataManager.SaveOnFinishedLevel(SpawnManager.Instance.LevelName, score: 1, playerBalanceOnLevelFinished);
            Debug.LogWarning(
                $"Spawned all of the enemies in this level <color=red>{SpawnManager.Instance.LevelName}</color>");
        }
        
        private void OnStartLevelEvent()
        {
            SpawnManager.Instance.StartSpawning();
            levelManager.LevelOnGoingChange();
        }

        private void OnEndLevel()
        {
            levelManager.LevelOnGoingChange();
            levelManager.UISceneLoadedEvent -= OnUISceneLoadedEvent;
        }

        private void OnPlayerDefeatedEvent()
        {
            var uiManager = GameObject.FindGameObjectWithTag("UIManager");
            uiManager.GetComponent<PopupManager>().ShowPopup(failLevelPopup);
        }

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
                OnEndLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("Reloading Scene");
                Time.timeScale = 1f;
            };
        }

        #endregion
    }
}
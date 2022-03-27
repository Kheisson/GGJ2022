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
            if (Instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(gameObject);

            _levelManager = Instantiate(levelManager, transform);
            dataManager = Instantiate(dataManager, transform);
            _playerInstance = FindObjectOfType<PlayerControl>();
            SubscribeToLevelEvents();
        }

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

        private void OnCreditUIEvent(int credit, string pickupName)
        {
        }

        private void OnFinishedSpawningEvent()
        {
            var playerBalanceOnLevelFinished = FindObjectOfType<PlayerStatsUI>().CoinBalance;
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
            FindObjectOfType<PopupManager>().ShowPopup(failLevelPopup);
        }

        public static bool Mute()
        {
            _isAudioMuted = !_isAudioMuted;
            var audioSource = Camera.main.GetComponent<AudioSource>();
            audioSource.mute = _isAudioMuted;
            return _isAudioMuted;
        }

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
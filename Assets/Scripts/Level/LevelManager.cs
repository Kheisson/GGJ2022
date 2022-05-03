using System.Collections.Generic;
using Core;
using Enemies;
using Player;
using Save;
using Spawn;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {

        #region Fields
        [SerializeField] private float backgroundSpeed = 12f;
        [SerializeField] private LevelProgressionSo levelProgressionSo;
        [SerializeField] private Popup failLevelPopup;
        [SerializeField] private List<Enemy> enemyTypes;

        private GameObject _backgroundGameObject;
        private bool _levelOnGoing = false;
        private const string UISceneName = "main";
        private PlayerControl _playerInstance;
        private SpawnManager _spawnManager;

        
        #endregion

        #region Methods

        private void Awake()
        {
            Time.timeScale = 1f;
            LoadUIScene();
        }

        private void Start()
        {
            _backgroundGameObject = GameObject.FindGameObjectWithTag("Background");
            _playerInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
            SubscribeToEvents();
        }

        private void Update()
        {
            if (!_levelOnGoing) return;
            _backgroundGameObject.transform.Translate(Vector3.forward * backgroundSpeed * Time.deltaTime);
        }

        private void SubscribeToEvents()
        {
            _playerInstance.PlayerMovedToStartingPosition += OnStartLevelEvent;
            _playerInstance.PlayerDefeatedEvent += OnPlayerDefeatedEvent;
            _spawnManager.FinishedSpawningEvent += OnFinishedSpawningEvent;
        }

        private void OnFinishedSpawningEvent()
        {
            var uiManager = GameObject.FindGameObjectWithTag("UIManager");
            var playerBalanceOnLevelFinished = uiManager.GetComponent<PlayerStatsUI>().CoinBalance;
            DataManager.SaveOnFinishedLevel(levelProgressionSo.name, score: 1, playerBalanceOnLevelFinished);
            Debug.LogWarning(
                $"Spawned all of the enemies in this level <color=red>{levelProgressionSo.name}</color>");
            GameManager.Instance.AddCompletedLevel(levelProgressionSo.name);
        }

        private void OnPlayerDefeatedEvent()
        {
            var uiManager = GameObject.FindGameObjectWithTag("UIManager");
            uiManager.GetComponent<PopupManager>().ShowPopup(failLevelPopup);
        }

        private void OnStartLevelEvent()
        {
            _spawnManager.StartSpawning();
            LevelOnGoingChange();
        }

        private void LoadUIScene()
        {
            var uiScene = SceneManager.LoadSceneAsync(UISceneName, LoadSceneMode.Additive);
            while (uiScene.isDone)
            {
                return;
            }
            uiScene.allowSceneActivation = true;
            Debug.Log("UI scene <color=green>loaded</color>");
            InitLevel();
        }
       
        private void LevelOnGoingChange() => _levelOnGoing = !_levelOnGoing;

        private void InitLevel()
        {
            _spawnManager = new GameObject("SpawnManager").AddComponent<SpawnManager>();
            _spawnManager.LevelProgressionSo = levelProgressionSo;
            _spawnManager.Enemies = enemyTypes;
        }

        #endregion


    }
}

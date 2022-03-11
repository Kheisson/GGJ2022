using System;
using Core;
using Player;
using Save;
using Spawn;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private DataManager dataManager;

    private static GameManager _instance;
    private PlayerControl _playerInstance;
    #endregion

    #region Events

    public Action StartLevelEvent;
    public Action EndLevelEvent;
    public Action<int> CreditUIEvent;

    #endregion

    #region Properties

    public static GameManager Instance => _instance;

    #endregion

    #region Methods

    private void Start()
    {
        if(_instance == null)
            _instance = this;
            
        DontDestroyOnLoad(gameObject);
        
        levelManager = Instantiate(levelManager, transform);
        dataManager = Instantiate(dataManager, transform);
        _playerInstance = FindObjectOfType<PlayerControl>();
        SubscribeToLevelEvents();
    }

    private void SubscribeToLevelEvents()
    {
        levelManager.UISceneLoadedEvent += OnUISceneLoadedEvent;
        _playerInstance.PlayerMovedToStartingPosition += OnStartLevelEvent;
        SpawnManager.Instance.FinishedSpawningEvent += OnFinishedSpawningEvent;
        CreditUIEvent += OnCreditUIEvent;
    }

    private void OnUISceneLoadedEvent()
    {
        Debug.Log("UI scene <color=green>loaded</color>");
    }

    private void OnCreditUIEvent(int credit)
    {
    }

    private void OnFinishedSpawningEvent()
    {
        var playerBalanceOnLevelFinished = FindObjectOfType<PlayerStatsUI>().CoinBalance;
        DataManager.SaveOnFinishedLevel(SpawnManager.Instance.LevelName, score: 1,playerBalanceOnLevelFinished);
        Debug.LogWarning($"Spawned all of the enemies in this level <color=red>{SpawnManager.Instance.LevelName}</color>");
    }

    private void OnStartLevelEvent()
    {
        SpawnManager.Instance.StartSpawning();
    }

    private void OnEndLevel()
    {
        levelManager.UISceneLoadedEvent -= OnUISceneLoadedEvent;
        Destroy(levelManager);
    }

    #endregion
}

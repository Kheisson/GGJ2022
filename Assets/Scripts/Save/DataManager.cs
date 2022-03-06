using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Save
{
    public class DataManager : MonoBehaviour
    {
        #region Fields

        public static DataManager Instance { get; private set; }
        private static string PlayerName;
        private static int BestScore;
        private static string BestScorePlayerName;

        private const string DataPath = "/Raptor/data.json";

        #endregion

        #region Methods

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public string GetPlayerName()
        {
            return PlayerName;
        }

        public void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        public string GetBestScorePlayerName()
        {
            return BestScorePlayerName;
        }

        public void SetBestScorePlayerName(string playerName)
        {
            BestScorePlayerName = playerName;
        }

        public int GetBestScore()
        {
            return BestScore;
        }

        public void SetBestScore(int newScore)
        {
            if(newScore > BestScore)
            {
                BestScore = newScore;
            }
        }

        #endregion

        #region Save/Load system

        [Serializable]
        private class SaveData
        {
            public string playerName;
            public int bestScore;
            public List<GameLevel> gameLevelList = new List<GameLevel>();
        }

        private class GameLevel
        {
            public string levelNum;
            public string bestScore;
        }

        public void SaveScore(int level)
        {
            SaveData save = new SaveData();
            save.playerName = PlayerName;
            save.bestScore = BestScore;

            string json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.persistentDataPath + DataPath, json);
        }

        public void LoadScore()
        {
            string path = File.ReadAllText(Application.persistentDataPath + DataPath);
            SaveData load = JsonUtility.FromJson<SaveData>(path);
            BestScorePlayerName = load.playerName;
            BestScore = load.bestScore;
        }

        #endregion
    }
}
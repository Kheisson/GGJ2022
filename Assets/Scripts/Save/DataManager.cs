using System;
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

        private const string DataPath = "/data.json";

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
            public GameLevel gameLevel;
            public int totalAmountCurrency;
        }

        [Serializable]
        private class GameLevel
        {
            public string levelName;
            //public int bestScore;
            public int curreny;

            public GameLevel(string levelName, int curreny)
            {
                this.levelName = levelName;
                this.curreny = curreny;
            }
        }

        public void SaveScore(string level, int currency)
        {
            SaveData save = new SaveData();
            /*save.playerName = PlayerName;
            save.bestScore = BestScore;*/
            save.gameLevel = new GameLevel(level, currency);

            string json = JsonUtility.ToJson(save);
            Debug.Log(json);
            File.WriteAllText(Application.persistentDataPath + DataPath, json);
        }

        public void LoadScore()
        {
            string path = File.ReadAllText(Application.persistentDataPath + DataPath);
            SaveData load = JsonUtility.FromJson<SaveData>(path);
            /*BestScorePlayerName = load.playerName;
            BestScore = load.bestScore;*/
        }

        #endregion
    }
}
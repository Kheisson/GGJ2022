using UnityEngine;

namespace Save
{
    public class DataManager : MonoBehaviour
    {
        #region Fields

        public static DataManager Instance { get; private set; }
        private static SaveData _saveData;
        private static FileHandler _fileHandler;
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

            _fileHandler = new FileHandler();
            _saveData = _fileHandler.LoadSaveData();
        }


        /// <summary>
        /// Saved data on device when a level has finished
        /// </summary>
        /// <param name="levelName">Level name as based in the level progression SO</param>
        /// <param name="score">Score achieved in the level</param>
        /// <param name="amountToCredit">Credits achieved in level</param>
        public static void SaveOnFinishedLevel(string levelName, int score, int amountToCredit)
        {
            _saveData.wallet += amountToCredit;
            var match = _saveData.levels.Find(level => level.levelName == levelName);
            if (match != null)
            {
                match.bestScore = match.bestScore < score ? score : match.bestScore;
            }
            else
            {
                _saveData.levels.Add(new GameLevel(levelName, score));
            }
            
            _fileHandler.SaveToStorage(_saveData);
        }
        #endregion
    }
}
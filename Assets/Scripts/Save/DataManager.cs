using System.Collections.Generic;
using Player;
using Shop;
using UnityEngine;

namespace Save
{
    public class DataManager : MonoBehaviour
    {
        #region Fields
        private static DataManager Instance { get; set; }
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

        //Updates balance and saves
        public static void SaveOnPurchase(int balance)
        {
            _saveData.wallet = balance;
            _fileHandler.SaveToStorage(_saveData);
            PlayerPrefs.Save();
        }

        //Saves the game when an object from the garage is equipped
        public static void SaveOnEquip(ShopItemType type, int itemID)
        {
            var bodyData = _saveData.bodyData;
            switch (type)
            {
                case ShopItemType.Cockpit:
                    bodyData.cockpit = itemID;
                    break;
                case ShopItemType.Tail:
                    bodyData.tail = itemID;
                    break;
                case ShopItemType.Wing:
                    bodyData.wings = itemID;
                    break;
                default:
                    Debug.Log($"ERROR: Item type unknown {type}");
                    break;
            }
            _fileHandler.SaveToStorage(_saveData);
        }

        public static PlayerBodyData GetPlayerBodyData()
        {
            return _saveData.bodyData;
        }

        public static int GetPlayerBalance()
        {
            return _saveData.wallet;
        }

        public static HashSet<int> GetClearedLevels()
        {
            var levels = new HashSet<int>();
            foreach (var level in _saveData.levels)
            {
                levels.Add(int.Parse(level.levelName.Split('l')[1]));
            }

            return levels;
        }

        //Returns the currently equipped item based on the item type
        public static int GetEquippedItem(ShopItemType type)
        {
            var bodyData = _saveData.bodyData;
            var itemID = -1;
            switch (type)
            {
                case ShopItemType.Cockpit:
                    itemID = bodyData.cockpit;
                    break;
                case ShopItemType.Tail:
                    itemID = bodyData.tail;
                    break;
                case ShopItemType.Wing:
                    itemID = bodyData.wings;
                    break;
                default:
                    Debug.Log($"ERROR: Item type unknown {type}");
                    break;
            }

            return itemID;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using Player;

namespace Save
{
    /// <summary>
    /// Serializable classes to store data
    /// levels - gamelevel comprised of best score and level name
    /// bodyData - comprised of strings to mark skins for the planes bodyData
    /// wallet - balance of player
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public List<GameLevel> levels;
        public PlayerBodyData bodyData;
        public int wallet;

        public SaveData()
        {
            wallet = 0;
            bodyData = new PlayerBodyData
            {
                cockpit = 0,
                wings = 0,
                tail = 0
            };
        }
    }

    [Serializable]
    public class GameLevel
    {
        public string levelName;
        public int bestScore;

        public GameLevel(string levelName, int bestScore)
        {
            this.levelName = levelName;
            this.bestScore = bestScore;
        }
    }
}
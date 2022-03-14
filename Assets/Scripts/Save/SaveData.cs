using System;
using System.Collections.Generic;
using Player;

namespace Save
{
    /// <summary>
    /// Serializable classes to store data
    /// levels - gamelevel comprised of best score and level name
    /// body - comprised of strings to mark skins for the planes body
    /// wallet - balance of player
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public List<GameLevel> levels;
        public PlayerBody body;
        public int wallet;

        public SaveData()
        {
            wallet = 0;
            body = new PlayerBody
            {
                cockpit = "default",
                wings = "default",
                tail = "default"
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
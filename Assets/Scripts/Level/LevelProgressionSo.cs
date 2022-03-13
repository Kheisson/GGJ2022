using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Stage", menuName = "Scriptables/New Stage")]
    public class LevelProgressionSo : ScriptableObject
    {
        [Range(0.01f,5f),Tooltip("Delays between spawns"),SerializeField] private float delaySpawnTimer;
        [Tooltip("Ordered list of enemies to spawn"),SerializeField] private List<GameObject> enemySpawnList = new List<GameObject>();
        [Tooltip("List that states the number of enemies to spawn"),SerializeField] private List<int> spawnEachAmount = new List<int>();
        
        public List<int> SpawnEachAmount => spawnEachAmount;
        public List<GameObject> EnemySpawnList => enemySpawnList;
        public float DelaySpawnTimer => delaySpawnTimer;
    }
}
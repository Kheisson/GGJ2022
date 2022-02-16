using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private LevelProgressionSo levelProgressionSo;
        private List<Enemy> _enemies = new List<Enemy>();
        private static SpawnManager _instance;
        private int _maxRandomX;
        

        public static SpawnManager Instance => _instance;

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            DontDestroyOnLoad(gameObject);
            _maxRandomX = -GameSettings.ScreenBoundaries.x > 30 ? 30 : 20;
        }

        private IEnumerator StartSpawningCoroutine()
        {
            for (int i = 0; i < levelProgressionSo.EnemySpawnList.Count; i++)
            {
                for (int j = 0; j < levelProgressionSo.SpawnEachAmount[i]; j++)
                {
                    var go = _enemies.Find( x => !x.gameObject.activeSelf && x.name.Contains(levelProgressionSo.EnemySpawnList[i].name));
                    var newPos = new Vector3(GenerateRandomX(), 0, 0);
                    if (go == null)
                    {
                        var spawnCandidate = Instantiate(levelProgressionSo.EnemySpawnList[i], Enemy.UniversalEnemyStartingPosition + newPos,
                            Quaternion.identity, transform).GetComponent<Enemy>();
                        _enemies.Add(spawnCandidate);
                    }
                    else
                    {
                        go.transform.position = Enemy.UniversalEnemyStartingPosition + newPos;
                        go.gameObject.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(levelProgressionSo.DelaySpawnTimer);
            }
        }

        private void StopSpawning() => StopAllCoroutines();

        private float GenerateRandomX()
        {
            _maxRandomX *= -1;
            return Random.Range(0f, _maxRandomX);
        }
        
        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());
    }
}
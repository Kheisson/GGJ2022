using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Core
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private LevelProgressionSo levelProgressionSo;
        private List<Enemy> _enemies = new List<Enemy>();
        private static SpawnManager _instance;

        public static SpawnManager Instance => _instance;

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            DontDestroyOnLoad(gameObject);
            var init = SpawnGrid.Grid;
        }

        private IEnumerator StartSpawningCoroutine()
        {
            for (int i = 0; i < levelProgressionSo.EnemySpawnList.Count; i++)
            {
                for (int j = 0; j < levelProgressionSo.SpawnEachAmount[i]; j++)
                {
                    var go = _enemies.Find( x => !x.gameObject.activeSelf && x.name.Contains(levelProgressionSo.EnemySpawnList[i].name));
                   
                    var newPos = SpawnGrid.GetOpenSpot();
                    if (newPos == Vector3.back) //Guard for no more open space left on grid
                        break;
                    
                    if (go == null)
                    {
                        var spawnCandidate = Instantiate(levelProgressionSo.EnemySpawnList[i], 
                            Enemy.UniversalEnemyStartingPosition + newPos,
                            Quaternion.identity, transform).GetComponent<Enemy>();
                        _enemies.Add(spawnCandidate);
                    }
                    else
                    {
                        go.transform.position = Enemy.UniversalEnemyStartingPosition + newPos;
                        go.gameObject.SetActive(true);
                    }
                }
                
                SpawnGrid.ClearGrid();
                
                yield return new WaitForSeconds(levelProgressionSo.DelaySpawnTimer);
            }
        }

        private void StopSpawning() => StopAllCoroutines();

        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());
    }
}
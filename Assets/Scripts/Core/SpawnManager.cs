using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class SpawnManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private LevelProgressionSo levelProgressionSo;
        private List<Enemy> _enemies = new List<Enemy>();
        private static SpawnManager _instance;
        private string[] _pickupTypes;

        public static SpawnManager Instance => _instance;

        #endregion

        #region Methods

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            DontDestroyOnLoad(gameObject);
            var init = SpawnGrid.Grid;
            _pickupTypes = Enum.GetNames(typeof(PickupType));
        }

        private IEnumerator StartSpawningCoroutine()
        {
            yield return new WaitForSeconds(levelProgressionSo.DelaySpawnTimer);
            
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

        public void GetPickables(Transform spawnPosition, bool pickRandom, string pickup = "Triangle", int amountToSpawn = 1)
        {
            if (pickRandom)
            {
                pickup = _pickupTypes[Random.Range(0, _pickupTypes.Length)];
            }

            for (var i = 0; i < amountToSpawn; i++)
            {
                var loadedPickup = Resources.Load<Pickup>($"Pickups/{pickup}");
                var go = Instantiate(loadedPickup, transform);
                go.SpawnOnDestroy(spawnPosition);
            }
        }

        #endregion
    }
}
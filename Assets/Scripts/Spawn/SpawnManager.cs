using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Enemies;
using Helpers;
using Level;
using Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private LevelProgressionSo levelProgressionSo;
        private List<Enemy> _enemies = new List<Enemy>();
        private static SpawnManager _instance;
        private string[] _pickupTypes;
        private float _pickupDropChance = 0.9f;
        #endregion
        
        #region Properties
        public static SpawnManager Instance => _instance;
        public string LevelName => levelProgressionSo.name;
        public int EnemyWaves => levelProgressionSo.EnemySpawnList.Count;
        public int EnemiesInLevel => _enemies.Count;

        #endregion
        
        #region Events
        public Action FinishedSpawningEvent;

        #endregion

        #region Methods

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            DontDestroyOnLoad(gameObject);
            SpawnGrid.Init(); 
            _pickupTypes = Enum.GetNames(typeof(PickupType));
        }
        /// <summary>
        /// Goes over the player progression SO and searches for gameObjects that are not active and that they have the correct type by name.
        /// If found it will assume a position on the grid, if not, one will be instantiated, enabled and added to the _enemies list.
        /// On completion the grid is cleaned and the FinishedSpawningEvent is Invoked.  
        /// </summary>
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
                        continue;
                    
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

            FinishedSpawningEvent?.Invoke();
        }

        private void StopSpawning() => StopAllCoroutines();

        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spawnPosition">The starting position of the pickable</param>
        /// <param name="pickRandom">If true one of the types will be picked at random</param>
        /// <param name="amountToCredit">Determines the credit amount the pickup will grant</param>
        /// <param name="pickup">A string representing the type - a list is found in PickupType</param>
        /// <param name="amountToSpawn">The amount of pickups to spawn</param>
        public void GetPickables(Transform spawnPosition, bool pickRandom, int amountToCredit, string pickup = "Triangle", int amountToSpawn = 1)
        {
            if (pickRandom)
            {
                pickup = _pickupTypes[Random.Range(0, _pickupTypes.Length)];
            }

            if (this.ReturnSuccessfulProbability(_pickupDropChance))
            {
                pickup = "Circle";
                amountToSpawn = 1;
            }
            
            for (var i = 0; i < amountToSpawn; i++)
            {
                var loadedPickup = Resources.Load<Pickup>($"Pickups/{pickup}");
                var go = Instantiate(loadedPickup, transform);
                go.SpawnOnDestroy(spawnPosition, amountToCredit);
                go.PickupPickedEvent += GameManager.Instance.CreditUIEvent;
            }
        }

        #endregion
    }
}
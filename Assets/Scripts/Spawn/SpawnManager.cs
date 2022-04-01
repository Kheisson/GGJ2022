using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Level;
using UnityEngine;

namespace Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        #region Fields
        
        private List<Enemy> _enemies = new List<Enemy>();
        private static SpawnManager _instance;

        #endregion

        #region Properties
        public LevelProgressionSo LevelProgressionSo { get; set; }
        #endregion
        
        #region Events
        public Action FinishedSpawningEvent;
        #endregion

        #region Methods

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            SpawnGrid.Init();
        }
        /// <summary>
        /// Goes over the player progression SO and searches for gameObjects that are not active and that they have the correct type by name.
        /// If found it will assume a position on the grid, if not, one will be instantiated, enabled and added to the _enemies list.
        /// On completion the grid is cleaned and the FinishedSpawningEvent is Invoked.  
        /// </summary>
        private IEnumerator StartSpawningCoroutine()
        {
            yield return new WaitForSeconds(LevelProgressionSo.DelaySpawnTimer);
            
            for (int i = 0; i < LevelProgressionSo.EnemySpawnList.Count; i++)
            {
                for (int j = 0; j < LevelProgressionSo.SpawnEachAmount[i]; j++)
                {
                    var go = _enemies.Find( x => !x.gameObject.activeSelf && x.name.Contains(LevelProgressionSo.EnemySpawnList[i].name));
                   
                    var newPos = SpawnGrid.GetOpenSpot();
                    if (newPos == Vector3.back) //Guard for no more open space left on grid
                        continue;
                    
                    if (go == null)
                    {
                        var spawnCandidate = Instantiate(LevelProgressionSo.EnemySpawnList[i], 
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
                
                yield return new WaitForSeconds(LevelProgressionSo.DelaySpawnTimer);
            }

            FinishedSpawningEvent?.Invoke();
        }

        private void StopSpawning() => StopAllCoroutines();
        

        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());

        #endregion
    }
}
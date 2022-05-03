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
        private List<GameObject> _enemies = new List<GameObject>();
        #endregion

        #region Properties
        public LevelProgressionSo LevelProgressionSo { get; set; }
        public List<Enemy> Enemies { get; set; }
        #endregion

        #region Events

        public Action FinishedSpawningEvent;

        #endregion

        #region Methods

        private void Awake()
        {
            SpawnGrid.Init();
        }

        /// <summary>
        /// Goes over the player progression SO and searches for gameObjects that are not active and that they have the correct type by name.
        /// If found it will assume a position on the grid, if not, one will be instantiated, enabled and added to the _enemies list.
        /// On completion the grid is cleaned and the FinishedSpawningEvent is Invoked.  
        /// </summary>
        private IEnumerator StartSpawningCoroutine()
        {
            var spawnUnitList = LevelProgressionSo.SpawnableUnits;
            for (int i = 0; i < spawnUnitList.Count; i++)
            {
                var go = _enemies.Find(x =>
                    !x.gameObject.activeSelf && x.name.Contains(spawnUnitList[i].enemyType.ToString()));

                var newPos = SpawnGrid.GetSpot(spawnUnitList[i].positionOnGrid);
                
                if (go == null)
                {
                    var enemyType = (int) spawnUnitList[i].enemyType;
                    var spawnCandidate = Instantiate(
                        Enemies[enemyType].gameObject,
                        Enemy.UniversalEnemyStartingPosition + newPos,
                        Quaternion.identity,
                        transform);
                    _enemies.Add(spawnCandidate) ;
                }
                else
                {
                    go.transform.position = Enemy.UniversalEnemyStartingPosition + newPos;
                    go.gameObject.SetActive(true);
                }
                
                yield return new WaitForSeconds(spawnUnitList[i].delay);
            }
            
            FinishedSpawningEvent?.Invoke();
        }

        private void StopSpawning() => StopAllCoroutines();


        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());

        #endregion
    }
}
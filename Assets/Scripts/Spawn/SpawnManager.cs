using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Level;
using Projectile;
using UnityEngine;

namespace Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        #region Fields
        private List<GameObject> _enemies = new List<GameObject>();
        private List<IProjectile> _projectiles = new List<IProjectile>();
        private GameObject _projectileBucket = null;
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
            _projectileBucket = new GameObject("ProjectileBucket");
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

                    spawnCandidate.GetComponent<Enemy>().Init(spawnUnitList[i].enemyType,
                        spawnUnitList[i].horizontalSpeed,
                        spawnUnitList[i].screenSectionStartMoving,
                        spawnUnitList[i].flip);
                    _enemies.Add(spawnCandidate) ;
                }
                else
                {
                    go.transform.position = Enemy.UniversalEnemyStartingPosition + newPos;
                    go.gameObject.SetActive(true);
                    go.GetComponent<Enemy>().StartMoving(spawnUnitList[i].horizontalSpeed,
                        spawnUnitList[i].screenSectionStartMoving,
                        spawnUnitList[i].flip);
                }
                
                yield return new WaitForSeconds(spawnUnitList[i].delay);
            }
            
            FinishedSpawningEvent?.Invoke();
        }

        private void StopSpawning() => StopAllCoroutines();
        public void StartSpawning() => StartCoroutine(StartSpawningCoroutine());

        public void FireProjectile(BaseProjectile projectile, Vector3 spawnPoint)
        {
            foreach (Transform child in _projectileBucket.transform)
            {
                if (child.gameObject.name.Contains(projectile.name) &&
                    !child.gameObject.activeInHierarchy)
                {
                    child.GetComponent<IProjectile>().Fire(spawnPoint);
                    return;
                }
            }
            
            var newBullet = Instantiate(projectile, _projectileBucket.transform);
            newBullet.GetComponent<IProjectile>().Fire(spawnPoint);
        }

        public void SetupProjectile(GameObject projectile)
        {
            var projectiles = ProjectileFactory.Instance.CreateWeaponQueue(projectile, 1, _projectileBucket.transform);
            _projectiles.AddRange(projectiles);
        }

        #endregion
    }
}
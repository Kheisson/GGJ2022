using System.Collections;
using System.Collections.Generic;
using Core;
using Projectile;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        #region Consts

        public static readonly Vector3 UniversalEnemyStartingPosition = new Vector3
        {
            x = 0,
            y = 60,
            z = 0
        };

        #endregion
        
        #region Fields

        [SerializeField] private EnemySetupSo enemySetupSo;
        [SerializeField] private string enemyContainerName;
        [SerializeField] private byte availableShots;
        private int _health;
        private bool _disableControl;
        private List<IProjectile> _projectiles;
        

        #endregion

        #region Methods

        private void OnEnable()
        {
            _health = enemySetupSo.EnemyHealth;
            _disableControl = true;
            if (_projectiles == null || _projectiles.Count == 0)
                CreateProjectileQueue();
        }

        private void Update()
        {
            if(!_disableControl)
                transform.position += Vector3.down * enemySetupSo.EnemySpeed;
        }

        private void Defeat()
        {
            gameObject.SetActive(false);
        }
        
        private void OnBecameInvisible() => gameObject.SetActive(false);

        private void OnBecameVisible()
        {
            _disableControl = false;
            Attack();
        }
        
        private IEnumerator EnemyAttackCoroutine()
        {
            while (true)
            {
                for (int i = 0; i < availableShots; i++)
                {
                    _projectiles[i].Fire(transform.position - (Vector3.up * 6));
                    yield return new WaitForSeconds(0.3f);
                }

                yield return new WaitForSeconds(3f);
            }
        }

        private void Attack()
        {
            StartCoroutine(EnemyAttackCoroutine());
        }

        private void CreateProjectileQueue()
        {
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(enemySetupSo.EnemyProjectile, availableShots, enemyContainerName);
        } 

        /// <summary>
        /// Disables the gameObject if health is eq/below zero
        /// </summary>
        /// <param name="damage">Passed from projectile</param>
        public void Damage(int damage)
        {
            _health -= damage;
            
            if(_health <= 0)
                Defeat();
        }
        
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using Core;
using Pickups;
using Projectile;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
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
        [SerializeField] private byte availableShots;

        private List<Pickup> _pickups = new List<Pickup>();
        private List<IProjectile> _projectiles;
        private int _health;
        private bool _disableControl;

        #endregion

        #region Methods

        private void Awake()
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationY |
                             RigidbodyConstraints.FreezeRotationZ;
        }

        private void OnEnable()
        {
            _health = enemySetupSo.EnemyHealth;
            _disableControl = true;
            if (_projectiles == null || _projectiles.Count == 0)
                CreateProjectileQueue();
        }

        private void FixedUpdate()
        {
            if(!_disableControl)
                transform.position += Vector3.down * enemySetupSo.EnemySpeed;
        }

        private void Defeat()
        {
            if (_pickups.Count == 0)
            {
                _pickups = SpawnManager.Instance.GetPickables(enemySetupSo.TypeOfPickup, enemySetupSo.PickupSpawnAmount);
            }

            foreach (var pickup in _pickups)
            {
                pickup.SpawnOnDestroy(transform);
            }
            
            gameObject.SetActive(false);
        }

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }

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
                    yield return new WaitForSeconds(_projectiles[i].FireRate);
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
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(enemySetupSo.EnemyProjectile, availableShots, transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains("Player"))
                return;
            
            if (other.TryGetComponent(typeof(IDamagable), out var subject))
                subject.GetComponent<IDamagable>().Damage(25);
            
            gameObject.SetActive(false);
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
using System.Collections;
using System.Collections.Generic;
using Core;
using Helpers;
using Projectile;
using Spawn;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
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
        [SerializeField] private int amountToCredit = 25;

        private List<IProjectile> _projectiles;
        private Collider _collider;
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
            _collider = GetComponent<Collider>();
            _collider.enabled = false;

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
            if (this.ReturnSuccessfulProbability())
            {
                SpawnManager.Instance.GetPickables(transform, false, amountToCredit);
            }

            gameObject.SetActive(false);
        }

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
            _collider.enabled = false;
        }

        private void OnBecameVisible()
        {
            _disableControl = false;
            Attack();
            _collider.enabled = true;
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

                yield return new WaitForSeconds(1f);
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

        //Will deactivate and damage player if collision occurs
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
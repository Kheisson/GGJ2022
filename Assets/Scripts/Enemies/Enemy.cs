using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Helpers;
using Pickups;
using Projectile;
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
        
        private const byte COLLISION_DAMAGE = 25;
        #endregion
        
        #region Fields

        [SerializeField] private EnemySetupSo enemySetupSo;

        private List<IProjectile> _projectiles;
        private Collider _collider;
        private Transform[] _children;
        private int _health;
        private float _pickupDropChance = 0.8f;
        private string _pickupName;
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
            _children = new Transform[enemySetupSo.AvaliableShots];
            _pickupName = Enum.GetName(typeof(PickupType), enemySetupSo.TypeOfPickup);
        }

        private void OnEnable()
        {
            _health = enemySetupSo.EnemyHealth;
            if (_projectiles == null || _projectiles.Count == 0)
                CreateProjectileQueue();
            
            if (_children[0] != null) return;
            for (var i = 0; i < transform.childCount; i++)
            {
                _children[i] = transform.GetChild(i);
            }
        }

        private void Update()
        {
            transform.position += Vector3.down * enemySetupSo.EnemySpeed * Time.deltaTime;
            
            if(transform.position.y < GameSettings.ScreenBoundaries.y - 5f)
                OffScreen();
        }

        private void Defeat()
        {
            SpawnPickup(1, enemySetupSo.AmountToCredit);

            transform.DetachChildren();
            gameObject.SetActive(false);
        }

        private void OffScreen()
        {
            gameObject.SetActive(false);
            _collider.enabled = false;
            StopAllCoroutines();
        }

        private void OnBecameVisible()
        {
            foreach (var child in _children)
            {
                child.SetParent(transform);
            }
            Attack();
            _collider.enabled = true;
        }
        
        private IEnumerator EnemyAttackCoroutine()
        {
            while (true)
            {
                for (int i = 0; i < enemySetupSo.AvaliableShots; i++)
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
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(enemySetupSo.EnemyProjectile, enemySetupSo.AvaliableShots, transform);
        }

        /// <summary>
        /// Spawns a pickup at enemy position
        /// </summary>
        /// <param name="amountToCredit">Determines the credit amount the pickup will grant</param>
        /// <param name="amountToSpawn">The amount of pickups to spawn</param>
        private void SpawnPickup(int amountToSpawn, int amountToCredit)
        {
            var pickup = _pickupName;
            if (this.ReturnSuccessfulProbability(_pickupDropChance))
            {
                pickup = "Circle";
                amountToSpawn = 1;
            }
            
            for (var i = 0; i < amountToSpawn; i++)
            {
                var loadedPickup = Resources.Load<Pickup>($"Pickups/{pickup}");
                var go = Instantiate(loadedPickup, transform);
                go.SpawnOnDestroy(transform, amountToCredit);
                go.PickupPickedEvent += GameManager.Instance.CreditUIEvent;
            }
        }

        //Will deactivate and damage player if collision occurs
        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains("Player"))
                return;
            
            if (other.TryGetComponent(typeof(IDamagable), out var subject))
                subject.GetComponent<IDamagable>().Damage(COLLISION_DAMAGE);
            
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
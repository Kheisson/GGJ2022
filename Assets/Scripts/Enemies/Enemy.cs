using System;
using System.Collections;
using Core;
using Helpers;
using MovementModules;
using Pickups;
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
        private Collider _collider;
        private int _health;
        private float _pickupDropChance = 0.8f;
        private string _pickupName;
        //Cached C++ elements
        private Transform _transform;
        private GameObject _gameObject;
        private MovingEnemy _moveModule;
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
            _pickupName = Enum.GetName(typeof(PickupType), enemySetupSo.TypeOfPickup);
            _transform = transform;
            _gameObject = gameObject;
        }

        private void OnEnable()
        {
            _health = enemySetupSo.EnemyHealth;
            if(_moveModule != null)
                _moveModule.StartMoving();
        }

        private void Update()
        {
            _transform.position += enemySetupSo.EnemySpeed * Time.deltaTime * Vector3.down;
            
            if(_transform.position.y < GameSettings.ScreenBoundaries.y - 5f)
                OffScreen();
        }

        private void Defeat()
        {
            SpawnPickup(1, enemySetupSo.AmountToCredit);

            _transform.DetachChildren();
            _gameObject.SetActive(false);
        }

        private void OffScreen()
        {
            _gameObject.SetActive(false);
            _collider.enabled = false;
            StopAllCoroutines();
        }

        private void OnBecameVisible()
        {
            Attack();
            _collider.enabled = true;
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator EnemyAttackCoroutine()
        {
            while (true)
            {
                for (int i = 0; i < enemySetupSo.AvaliableShots; i++)
                {
                    GameManager.Spawner.FireProjectile(enemySetupSo.EnemyProjectile,_transform.position - (Vector3.up * 6));
                }
                yield return new WaitForSecondsRealtime(enemySetupSo.EnemyProjectile.FireRate);
            }
        }

        private void Attack()
        { 
            StartCoroutine(EnemyAttackCoroutine());
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
                var go = Instantiate(loadedPickup, _transform);
                go.SpawnOnDestroy(_transform, amountToCredit);
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
            
            _gameObject.SetActive(false);
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
        /// <summary>
        /// Initiates the enemy to the correct behavioural style
        /// </summary>
        /// <param name="enemyType"></param>
        public void Init(EnemyType enemyType)
        {
            _moveModule = MoveModule.AddMovingModule(enemyType, transform);
            GameManager.Spawner.SetupProjectile(enemySetupSo.EnemyProjectile.gameObject);
            if(_moveModule != null)
                _moveModule.StartMoving();
        }
        
        #endregion
    }
}
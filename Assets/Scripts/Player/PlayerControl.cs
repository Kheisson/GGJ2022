using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using Projectile;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))] 
    public class PlayerControl : MonoBehaviour, IDamagable
    {
        #region Consts
        private Vector2 _velocity = Vector2.zero;
        private readonly float _screenBoundaryDivider = 1.3f;
        #endregion

        #region Fields

        [SerializeField] private PlayerSettingsSo playerSettingsSo;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private Ease enteringSceneEase;
        private List<IProjectile> _projectiles;
        private Rigidbody _playerRb;
        private Vector3 _screenBoundaries;
        private Camera _mainCamera;
        private float _sizeOffset;
        private float _delayFire;
        private float _fireRate;
        private int _playerHealth;
        private bool _disableControl = true;
        
        //Dotween configuration
        [Header("Dotween Configuration")]
        [SerializeField] private float dotweenRotationDuration = 0.5f;
        [SerializeField] private Ease planeRotationEase;
        [SerializeField] private Vector3 _rotateVector = new Vector3(0f, 30f, 0f);
        public event Action PlayerDefeatedEvent;
        public event Action PlayerMovedToStartingPosition;
        public event Action<int> PlayerDamagedEvent;

        #endregion

        #region Properties
        public bool ControlsDisabled => _disableControl;

        #endregion
        
        #region Methods

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;
            _screenBoundaries = GameSettings.ScreenBoundaries;
            _sizeOffset = GetComponent<Collider>().bounds.extents.x;
            _playerHealth = playerSettingsSo.MaxHealth;
        }

        private void Start()
        {
            transform.DOMoveY(_screenBoundaries.y / _screenBoundaryDivider, 3f).SetEase(enteringSceneEase).OnComplete(() =>
            {
                _disableControl = false;
                PlayerMovedToStartingPosition?.Invoke();
            });
        }
        
        private void FixedUpdate()
        {
            if (_disableControl) return;
            ClampPosition();
            HandleMovement();
        }

        /// <summary>
        /// Will spawn a new list of projectiles if empty
        /// Delays fire based on projectile FireRate
        /// </summary>
        private void HandleAttack()
        {
            if (_projectiles == null || _projectiles.Count == 0)
                CreateProjectileQueue();

            _fireRate = _projectiles[0].FireRate;
            _delayFire += Time.deltaTime;
            
            if (!(_delayFire > _fireRate)) return;
            _delayFire = 0;
            var projectile = _projectiles.First(projectile => !projectile.IsActive);
            projectile.Fire(projectileSpawnPosition.position);
        }

        /// <summary>
        /// Creates a queue based on the currentWeapon
        /// </summary>
        private void CreateProjectileQueue()
        {
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(playerSettingsSo.CurrentWeapon, playerSettingsSo.SpawnCount, projectileSpawnPosition);
        }

        /// <summary>
        /// Gets the first touch and converts it to world point
        /// Will handle attack as long as finger is on screen
        /// </summary>
        private void HandleMovement()
        {
            //Mobile Controls for general movement
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            var point = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -80.0f));
            point = new Vector2(-point.x, -point.y);
            transform.position = Vector2.SmoothDamp(transform.position, point, ref _velocity, playerSettingsSo.MobileSpeed);
            
            //If there is a finger on screen, shoot
            if(touch.phase != TouchPhase.Ended)
                HandleAttack();
            
            //If there was a swipe, calculate swipe and apply rotation
            if (touch.phase == TouchPhase.Moved)
            {
                HandleRotation(point.x);
            }
        }

        /// <summary>
        /// Rotates the body of the plain based on the swipe of the player
        /// </summary>
        /// <param name="pointX"></param>
        private void HandleRotation(float pointX)
        {
            if (DOTween.IsTweening(transform))
                return;
            if (pointX > 1.0f)
            {
                transform.DORotate(_rotateVector * -1f, dotweenRotationDuration).SetEase(planeRotationEase).OnComplete(() =>
                {
                    transform.DORotate(Vector3.zero, dotweenRotationDuration);
                });
            }
            else if (pointX < -1.0f)
            {
                transform.DORotate(_rotateVector, dotweenRotationDuration).SetEase(planeRotationEase).OnComplete(() =>
                {
                    transform.DORotate(Vector3.zero, dotweenRotationDuration);
                });
            }
        }

        /// <summary>
        /// Clamps x position based on screen boundaries 
        /// </summary>
        private void ClampPosition()
        {
            var position = transform.position;
            var clampedPosition = new Vector3
            {
                x = Mathf.Clamp(position.x, _screenBoundaries.x + _sizeOffset, -_screenBoundaries.x - _sizeOffset),
                y = Mathf.Clamp(position.y, _screenBoundaries.y + _sizeOffset, -_screenBoundaries.y - _sizeOffset),
                z = position.z
            };
            transform.position = clampedPosition;
        }
        
        private void PlayerDefeated()
        {
            Debug.Log("Player died!");
            //transform.DOKill();
        }
        
        public void Damage(int damage)
        {
            if (_playerHealth - damage <= 0)
            {
                PlayerDefeated();
            }
            else
            {
                PlayerDamagedEvent?.Invoke(damage);
                _playerHealth -= damage;
            }
        }
        #endregion
    }
}
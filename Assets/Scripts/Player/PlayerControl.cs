using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        #endregion

        #region Fields

        [SerializeField] private PlayerSettingsSo playerSettingsSo;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private Ease enteringSceneEase;
        private List<IProjectile> _projectiles;
        private Rigidbody _playerRb;
        private Camera _mainCamera;
        private float _sizeOffset;
        private float _delayFire;
        private float _fireRate;
        private bool _disableControl = true;
        private Transform _transform;
        
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
            _sizeOffset = GetComponent<Collider>().bounds.extents.x;
            _transform = transform;
            playerSettingsSo.Replenish();
        }

        private void Start()
        { 
            _transform.DOMoveY(GameSettings.ScreenBoundaries.y / GameSettings.ScreenBoundaryDivider, 3f).SetEase(enteringSceneEase).OnComplete(() =>
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
            point = new Vector2(-point.x, -point.y + playerSettingsSo.YOffset);
            
            //Bufferzone on top of screen
            if (point.y > GameSettings.ScreenBoundariesTop)
                return;
            _transform.position = Vector2.SmoothDamp(_transform.position, point, ref _velocity, playerSettingsSo.MobileSpeed);
            
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
            if (DOTween.IsTweening(_transform))
                return;
            if (pointX > 1.0f)
            {
                _transform.DORotate(_rotateVector * -1f, dotweenRotationDuration).SetEase(planeRotationEase).OnComplete(() =>
                {
                    _transform.DORotate(Vector3.zero, dotweenRotationDuration);
                });
            }
            else if (pointX < -1.0f)
            {
                _transform.DORotate(_rotateVector, dotweenRotationDuration).SetEase(planeRotationEase).OnComplete(() =>
                {
                    _transform.DORotate(Vector3.zero, dotweenRotationDuration);
                });
            }
        }

        /// <summary>
        /// Clamps x position based on screen boundaries 
        /// </summary>
        private void ClampPosition()
        {
            var position = _transform.position;
            var clampedPosition = new Vector3
            {
                x = Mathf.Clamp(position.x, GameSettings.ScreenBoundaries.x + _sizeOffset, -GameSettings.ScreenBoundaries.x - _sizeOffset),
                y = Mathf.Clamp(position.y, GameSettings.ScreenBoundaries.y + _sizeOffset, -GameSettings.ScreenBoundaries.y - _sizeOffset),
                z = position.z
            };
            _transform.position = clampedPosition;
        }
        
        private void PlayerDefeated()
        {
            Debug.Log("Player died!");
            PlayerDefeatedEvent?.Invoke();
            //transform.DOKill();
        }
        
        public void Damage(int damage)
        {
            if (playerSettingsSo.PlayerHealth - damage <= 0)
            {
                PlayerDefeated();
            }
            else
            {
                PlayerDamagedEvent?.Invoke(damage);
                playerSettingsSo.Replenish(-damage);
            }
        }

        public void Damage(int damage, bool alternative)
        {
            if(alternative)
                playerSettingsSo.Replenish(damage);
            else
                Damage(-damage);
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(GameSettings.ScreenBoundaries.x * 2f, -GameSettings.ScreenBoundaries.y * 100,0));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(-GameSettings.ScreenBoundaries.x, 
                GameSettings.ScreenBoundaries.y / GameSettings.ScreenBoundaryDivider),new Vector2(GameSettings.ScreenBoundaries.x, 
                GameSettings.ScreenBoundaries.y / GameSettings.ScreenBoundaryDivider) );
        }
#endif
    }
}
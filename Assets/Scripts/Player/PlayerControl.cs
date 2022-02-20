using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Projectile;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))] 
    public class PlayerControl : MonoBehaviour
    {
        #region Consts
        private Vector2 _velocity = Vector2.zero;
        #endregion

        #region Fields
        
        [SerializeField] private float mobileSpeed = 0.5f;
        [SerializeField] private GameObject currentWeapon;
        [SerializeField] private Transform projectileSpawnPosition;
        private List<IProjectile> _projectiles;
        private Rigidbody _playerRb;
        private Vector3 _screenBoundaries;
        private Camera _mainCamera;
        private float _sizeOffset;
        private float _delayFire;
        private float _fireRate;
        private float _screenBoundryDivider = 1.3f;
        private int _spawnCount = 20;
        private bool _disableControl = true;

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
        }

        private void Start()
        {
            StartCoroutine(MoveToStartingPosition(new Vector3(0, _screenBoundaries.y / _screenBoundryDivider, 0)));
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
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(currentWeapon, _spawnCount, transform);
        }

        /// <summary>
        /// Gets the first touch and converts it to world point
        /// Will handle attack as long as finger is on screen
        /// </summary>
        private void HandleMovement()
        {
            //Mobile Controls
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            var point = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -80.0f));
            point = new Vector2(-point.x, -point.y);
            transform.position = Vector2.SmoothDamp(transform.position, point, ref _velocity, mobileSpeed);
            if(touch.phase != TouchPhase.Ended)
                HandleAttack();
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
        
        /// <summary>
        /// Lerps player position to the starting position and enabled controls
        /// </summary>
        /// <param name="startingPosition"> Sets the position the craft should start at</param>
        private IEnumerator MoveToStartingPosition(Vector3 startingPosition)
        {
            while (Vector3.Distance(startingPosition, transform.position) > 0.1f) 
            {
                transform.position = Vector3.Lerp(transform.position, startingPosition, Time.deltaTime);
                yield return null;
            }

            transform.position = new Vector3(0, _screenBoundaries.y / _screenBoundryDivider, 0);
            _disableControl = false;
            SpawnManager.Instance.StartSpawning();
        }

        #endregion
    }
}
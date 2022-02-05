using System.Collections.Generic;
using System.Linq;
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
        private List<IProjectile> _projectiles;
        private Rigidbody _playerRb;
        private Vector3 _screenBoundaries;
        private Camera _mainCamera;
        private float _sizeOffset;
        private float _delayFire;
        private float _fireRate;
        

        #endregion

        #region Methods

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;
            _screenBoundaries = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            _sizeOffset = GetComponent<Collider>().bounds.extents.x;
        }

        private void FixedUpdate()
        {
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
            projectile.Fire(transform.position);
        }

        /// <summary>
        /// Creates a queue based on the currentWeapon
        /// </summary>
        private void CreateProjectileQueue()
        {
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(currentWeapon);
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
            var point = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f));
            point = new Vector2(-point.x, _screenBoundaries.y / 2);
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
                y = position.y,
                z = position.z
            };
            transform.position = clampedPosition;
        }

        #endregion
    }
}
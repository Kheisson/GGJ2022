using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))] 
    public class PlayerMovement : MonoBehaviour
    {
        #region Consts
        
        #endregion

        #region Variables

        [SerializeField] private float speed = 250f;
        [SerializeField] private float sizeOffset = .6f;
        private Rigidbody _playerRb;
        private Vector3 _screenBoundaries;
        private Camera _mainCamera;

        #endregion

        #region Methods

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;
            _screenBoundaries = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        }

        private void FixedUpdate()
        {
            ClampPosition();
            HandleMovement();
        }

        private void HandleMovement()
        {
#if UNITY_EDITOR
            var moveDirection = new Vector3
            {
                x = Input.GetAxis("Horizontal") * speed * Time.deltaTime,
                y = 0,
                z = 0
            };
            _playerRb.velocity = moveDirection;
#elif UNITY_ANDROID || UNITY_IOS
            //Mobile Controls
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                var touchPosition = _mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                _playerRb.MovePosition(new Vector2(touchPosition.x * 10f * Time.deltaTime, 0));
            }
#endif
        }

        private void ClampPosition()
        {
            var position = transform.position;
            var clampedPosition = new Vector3
            {
                x = Mathf.Clamp(position.x, _screenBoundaries.x + sizeOffset, -_screenBoundaries.x - sizeOffset),
                y = position.y,
                z = position.z
            };
            transform.position = clampedPosition;
        }

        #endregion
    }
}
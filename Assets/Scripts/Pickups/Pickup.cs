using System;
using Core;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float decayTimer = 5f;
        [SerializeField] private float speed = 5f;

        [Header("DOTween Configuration")] [SerializeField]
        private float shakeDuration;

        [SerializeField] private float shakeStrength;
        [SerializeField] private int shakeRepeats;
        [SerializeField] private Ease shakeEase;
        [SerializeField] private float rotationSpeed = 1f;

        private readonly Vector3 _rotationTween = new Vector3(15f, 0, 360f);
        private Vector3 _floatPosition;
        private int _amountToCredit;


        public Action<int, string> PickupPickedEvent;

        #endregion

        #region Methods

        /// <summary>
        ///     Assigns the amount one pickup will credit and sets the spawn position of the pickup
        /// </summary>
        /// <param name="spawnPos">Starting position of the object</param>
        /// <param name="credit">Credit due to player if he interacts with object</param>
        public void SpawnOnDestroy(Transform spawnPos, int credit)
        {
            _amountToCredit = credit;
            transform.position = spawnPos.position;
        }

        private void FixedUpdate()
        {
            transform.position += Time.fixedDeltaTime * speed * _floatPosition;
            if(Mathf.Abs(transform.position.x) > Mathf.Abs(GameSettings.ScreenBoundaries.x))
                _floatPosition.x *= -1;
        }

        // Dotween integration for floating and shacking 
        private void OnEnable()
        {
            _floatPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, -2f), 0f);
            Destroy(gameObject, decayTimer);
            transform.DOShakeScale(shakeDuration, shakeStrength).SetDelay(decayTimer * 0.5f).SetEase(shakeEase)
                .SetLoops(shakeRepeats);
            transform.DORotate(_rotationTween, rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void OnBecameInvisible()
        {
            transform.DOKill();
            Destroy(this);
        }

        // Will credit user and destroy object when it collides with player
        private void OnTriggerEnter(Collider other)
        {
            PickupPickedEvent?.Invoke(_amountToCredit, gameObject.name);
            transform.DOKill();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            PickupPickedEvent -= GameManager.Instance.CreditUIEvent;
        }

        #endregion
    }
}
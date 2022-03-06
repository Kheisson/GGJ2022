using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int amountToCredit;
        [SerializeField] private float decayTimer = 5f;

        [Header("DOTween Configuration")] 
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeStrength;
        [SerializeField] private int shakeRepeats;
        [SerializeField] private Ease shakeEase;

        private Vector3 _floatPosition;

        public Action<int> PickupPickedEvent;

        #endregion

        #region Methods

        public void SpawnOnDestroy(Transform spawnPos)
        {
            transform.position = spawnPos.position;
        }

        private void FixedUpdate()
        {
            transform.position += _floatPosition * Time.fixedDeltaTime;
        }

        private void OnEnable()
        {
            _floatPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, -2f), 0f);
            Destroy(gameObject,decayTimer);
            transform.DOShakeScale(shakeDuration, shakeStrength).SetDelay(decayTimer * 0.5f).SetEase(shakeEase).SetLoops(shakeRepeats);
        }

        private void OnBecameInvisible()
        {
            transform.DOKill();
            Destroy(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            PickupPickedEvent?.Invoke(amountToCredit);
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
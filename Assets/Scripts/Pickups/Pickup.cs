using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private int amountToCredit;
        [SerializeField] private float decayTimer = 5f;

        private Vector3 _floatPosition;

        public Action<int> PickupPickedEvent;

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
        }

        private void OnBecameInvisible() => Destroy(this);
        private void OnTriggerEnter(Collider other)
        {
            PickupPickedEvent?.Invoke(amountToCredit);
            Destroy(gameObject);
        }
    }
}
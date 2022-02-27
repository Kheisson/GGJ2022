using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private int amountToCredit;

        private Vector3 _floatPosition;

        public Action<int> PickupPickedEvent;

        public void SpawnOnDestroy(Transform spawnPos)
        {
            transform.position = spawnPos.position;
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            transform.position += _floatPosition * Time.fixedDeltaTime * Random.Range(2f, 6f);
        }

        private void OnEnable()
        {
            _floatPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, -2f), 0f);
        }

        private void OnBecameInvisible() => gameObject.SetActive(false);

        private void OnTriggerEnter(Collider other)
        {
            PickupPickedEvent?.Invoke(amountToCredit);
            gameObject.SetActive(false);
        }
    }
}
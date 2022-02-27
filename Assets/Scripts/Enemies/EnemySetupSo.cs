using Pickups;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptables/New Enemy")]
    public class EnemySetupSo : ScriptableObject
    {
        #region Fields

        [SerializeField] private int health;
        [SerializeField] private float speed;
        [SerializeField] private GameObject weapon;
        [SerializeField] private int pickupSpawnAmount = 3;
        [SerializeField] private PickupType pickupType = PickupType.Triangle;

        #endregion

        #region Properties

        public int EnemyHealth => health;
        public float EnemySpeed => speed;
        public GameObject EnemyProjectile => weapon;

        public int PickupSpawnAmount => pickupSpawnAmount;
        public PickupType TypeOfPickup => pickupType;

        #endregion
    }
}
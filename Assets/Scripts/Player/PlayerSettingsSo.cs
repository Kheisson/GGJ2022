using System;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Player Setting", menuName = "Scriptables/New Player Setting")]
    public class PlayerSettingsSo : ScriptableObject
    {
        [SerializeField] private GameObject currentWeapon;
        [SerializeField] private float mobileSpeed; 
        [SerializeField] private int spawnCount;
        [SerializeField] private int maxHealth = 900;

       

        public GameObject CurrentWeapon
        {
            get => currentWeapon;
            set => currentWeapon = value;
        }

        public int SpawnCount
        {
            get => spawnCount;
            set => spawnCount = value;
        }

        public float MobileSpeed
        {
            get => mobileSpeed;
            set => mobileSpeed = value;
        }

        public int MaxHealth => maxHealth;
        

        public void IncreaseMaxHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;
        }
        

    }
}
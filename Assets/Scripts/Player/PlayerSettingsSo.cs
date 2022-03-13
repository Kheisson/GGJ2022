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
        [SerializeField] private int playerHealth;

       

        public GameObject CurrentWeapon => currentWeapon;

        public int SpawnCount => spawnCount;

        public float MobileSpeed => mobileSpeed;

        public int MaxHealth => maxHealth;

        public int PlayerHealth => playerHealth;


        public void IncreaseMaxHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;
        }

        public void Replenish(int health)
        {
            playerHealth += health;
            if (playerHealth > maxHealth)
                playerHealth = maxHealth;
        }

        public void Replenish()
        {
            playerHealth = maxHealth;
        }
    }
}
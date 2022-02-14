using Projectile;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptables/New Enemy")]
    public class EnemySetupSo : ScriptableObject
    {
        #region Fields

        [SerializeField] private int health;
        [SerializeField] private float speed;
        [SerializeField] private BaseProjectile weapon;

        #endregion

        #region Properties

        public int EnemyHealth => health;
        public float EnemySpeed => speed;
        public BaseProjectile EnemyProjectile => weapon;

        #endregion
    }
}
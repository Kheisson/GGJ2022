using Projectile;
using UnityEngine;

namespace Enemies
{
    public class EnemySetupSO : ScriptableObject
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
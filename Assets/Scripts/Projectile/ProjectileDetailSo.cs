using UnityEngine;

namespace Projectile
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Scriptables/New Projectile")]
    public class ProjectileDetailSo : ScriptableObject
    {

        #region Fields

        [SerializeField] private float projectileSpeed;
        [SerializeField] private float fireRate;
        [SerializeField] private int damage;

        #endregion

        #region Properties

        public float ProjectileSpeed => projectileSpeed;
        public float FireRate => fireRate;
        public int Damage => damage;

        #endregion
        
    }
}
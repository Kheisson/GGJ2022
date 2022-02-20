using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class ProjectileFactory
    {
        #region Fields
        private static ProjectileFactory _instance;
        #endregion

        #region Properties

        public static ProjectileFactory Instance
        {
            get { return _instance ??= new ProjectileFactory(); }
        }

        #endregion

        #region Methods

        public List<IProjectile> CreateWeaponQueue(GameObject projectile, int spawnCount, Transform parentObject)
        {
            var weapons = new List<IProjectile>();
            for (int i = 0; i < spawnCount; i++)
            {
                var instance = Object.Instantiate(projectile, parentObject);
                weapons.Add(instance.GetComponent<IProjectile>());
            }
            return weapons;
        }

        #endregion
    }
}
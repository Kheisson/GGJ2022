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

        public List<IProjectile> CreateWeaponQueue(GameObject projectile, int spawnCount, string containerName)
        {
            var weapons = new List<IProjectile>();
            var parent = new GameObject(containerName);
            for (int i = 0; i < spawnCount; i++)
            {
                var instance = Object.Instantiate(projectile, parent.transform);
                weapons.Add(instance.GetComponent<IProjectile>());
            }
            return weapons;
        }

        #endregion
    }
}
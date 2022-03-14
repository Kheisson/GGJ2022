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

        /// <summary>
        /// Factory for projectiles
        /// </summary>
        /// <param name="projectile">The mold of the projectile in gameobject form</param>
        /// <param name="spawnCount">Amount of projectiles to spawn</param>
        /// <param name="parentObject">Parent transform in hierarchy</param>
        /// <returns></returns>
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
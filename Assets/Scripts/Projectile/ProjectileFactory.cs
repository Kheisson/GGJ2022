using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class ProjectileFactory
    {
        #region Fields
        
        private static ProjectileFactory _instance;
        private static byte _spawnCount = 10;

        #endregion

        #region Properties

        public static ProjectileFactory Instance
        {
            get { return _instance ??= new ProjectileFactory(); }
        }

        public byte SpawnCount
        {
            get => _spawnCount;
            set => _spawnCount = value;
        }

        #endregion

        #region Methods

        public List<IProjectile> CreateWeaponQueue(GameObject projectile)
        {
            var weapons = new List<IProjectile>();
            for (int i = 0; i < _spawnCount; i++)
            {
                var instance = Object.Instantiate(projectile);
                weapons.Add(instance.GetComponent<IProjectile>());
            }
            return weapons;
        }

        #endregion
    }
}
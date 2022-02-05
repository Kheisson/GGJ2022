using UnityEngine;

namespace Projectile
{
    public interface IProjectile
    {
        public bool IsActive { get; }
        public float FireRate { get; }
        public void Fire(Vector2 startingPoint);
    }
}
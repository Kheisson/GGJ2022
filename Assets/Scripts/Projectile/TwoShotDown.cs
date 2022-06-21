using Core;
using UnityEngine;

namespace Projectile
{
    public class TwoShotDown : BaseProjectile
    {
        [SerializeField] private BaseProjectile decoy;
        [SerializeField] private float decoySpawnDelay;
        private Vector2 _decoyStartingPoint;
        public override void Fire(Vector2 startingPoint)
        {
            base.Fire(startingPoint);
            _decoyStartingPoint = startingPoint;
            Invoke(nameof(CreateDecoy), decoySpawnDelay);
        }

        private void CreateDecoy()
        {
            var copy = Instantiate(decoy, GameManager.Spawner.transform).GetComponent<BaseProjectile>();
            copy.SetDecoy(ProjectileSpeed, Damage);
            copy.Fire(_decoyStartingPoint);
        }
    }
}
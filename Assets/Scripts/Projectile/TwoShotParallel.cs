using Core;
using UnityEngine;

namespace Projectile
{
    public class TwoShotParallel : BaseProjectile
    {
        [SerializeField] private BaseProjectile decoy;
        [SerializeField] private float decoySpawnRange;
        public override void Fire(Vector2 startingPoint)
        {
            var leftBullet = startingPoint - (Vector2.right * decoySpawnRange);
            var rightBullet = startingPoint + (Vector2.right * decoySpawnRange);
            var copy = Instantiate(decoy, GameManager.Spawner.transform).GetComponent<BaseProjectile>();
            base.Fire(leftBullet);
            copy.Fire(rightBullet);
        }
    }
}
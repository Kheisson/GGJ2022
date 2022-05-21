using Core;
using UnityEngine;

namespace Projectile
{
    public class TwoShotDown : BaseProjectile
    {
        [SerializeField] private BaseProjectile decoy;
        private Vector2 _decoyStartingPoint;
        public override void Fire(Vector2 startingPoint)
        {
            base.Fire(startingPoint);
            _decoyStartingPoint = startingPoint;
            Invoke(nameof(CreateDecoy), 0.5f);
        }

        private void CreateDecoy()
        {
            var copy = Instantiate(decoy, GameManager.Spawner.transform).GetComponent<BaseProjectile>();
            copy.Fire(_decoyStartingPoint);
        }
    }
}
using Core;
using UnityEngine;

namespace Projectile
{
    public class FourShot : BaseProjectile
    {
        [SerializeField] private BaseProjectile decoy;
        public override void Fire(Vector2 startingPoint)
        {
            //110 when x is pos, 70 when x is neg
            var rotationOfBulletRight = Quaternion.Euler(new Vector3(70f, 90f,90f));
            var rotationOfBulletLeft = Quaternion.Euler(new Vector3(70f, -90f,-90f));
            
            transform.rotation = rotationOfBulletRight;
            var copy = Instantiate(decoy, GameManager.Spawner.transform).GetComponent<BaseProjectile>();
            copy.transform.rotation = rotationOfBulletLeft;
            base.Fire(startingPoint);
            copy.Fire(startingPoint);
        }
    }
}
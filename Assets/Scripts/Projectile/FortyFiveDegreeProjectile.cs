using UnityEngine;

namespace Projectile
{
    public class FortyFiveDegreeProjectile : BaseProjectile
    {
        public override void Fire(Vector2 startingPoint)
        {
            base.Fire(startingPoint);
            //110 when x is pos, 70 when x is neg
            var x = startingPoint.x > 0 ? 110 : 70;
            transform.rotation = Quaternion.Euler(new Vector3(x, 90f,90f));
        }
    }
}
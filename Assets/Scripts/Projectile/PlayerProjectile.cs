using Core;
using UnityEngine;

namespace Projectile
{
    public class PlayerProjectile : BaseProjectile
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(typeof(IDamagable), out var subject))
            {
                subject.GetComponent<IDamagable>().Damage(projectileDetails.Damage);
                gameObject.SetActive(false);
            }
        }
    }
}
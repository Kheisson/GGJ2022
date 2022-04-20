using Core;
using Enemies;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class BaseProjectile : MonoBehaviour, IProjectile
    {
        #region Fields

        [SerializeField] protected ProjectileDetailSo projectileDetails;
        private Rigidbody _projectileRb;

        #endregion

        #region Properties

        public bool IsActive => gameObject.activeInHierarchy;
        public float FireRate => projectileDetails.FireRate;
        public int Damage => projectileDetails.Damage;

        #endregion
        
        #region Methods

        private void Awake()
        {
            _projectileRb = GetComponent<Rigidbody>();
            GetComponent<Collider>().isTrigger = true;
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            _projectileRb.velocity = transform.forward * projectileDetails.ProjectileSpeed;
        }

        private void OnBecameInvisible() => gameObject.SetActive(false);

        public virtual void Fire(Vector2 startingPoint)
        {
            transform.position = startingPoint;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(typeof(IDamagable), out var subject))
            {
                if (subject.GetComponent<Enemy>() != null) //Guards against enemy self-harm
                    return;

                subject.GetComponent<IDamagable>().Damage(projectileDetails.Damage);
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
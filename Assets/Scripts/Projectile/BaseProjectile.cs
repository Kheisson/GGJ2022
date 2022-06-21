using Core;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class BaseProjectile : MonoBehaviour, IProjectile
    {
        #region Fields

        [SerializeField] protected ProjectileDetailSo projectileDetails;
        private Rigidbody _projectileRb;
        private int _damage;
        private float _speed;

        #endregion

        #region Properties

        public bool IsActive => gameObject.activeInHierarchy;
        public float FireRate => projectileDetails.FireRate;
        public int Damage => projectileDetails.Damage;
        public float ProjectileSpeed => projectileDetails.ProjectileSpeed;

        #endregion
        
        #region Methods

        private void Awake()
        {
            _projectileRb = GetComponent<Rigidbody>();
            GetComponent<Collider>().isTrigger = true;
            gameObject.SetActive(false);
            _speed = projectileDetails.ProjectileSpeed;
            _damage = projectileDetails.Damage;

        }

        private void FixedUpdate()
        {
            _projectileRb.velocity = transform.forward * _speed;
        }

        private void OnBecameInvisible() => gameObject.SetActive(false);

        public virtual void Fire(Vector2 startingPoint)
        {
            transform.position = startingPoint;
            gameObject.SetActive(true);
        }

        public void SetDecoy(float speed, int damage)
        {
            _speed = speed;
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(typeof(IDamagable), out var subject))
            {
                subject.GetComponent<IDamagable>().Damage(_damage);
                gameObject.SetActive(false);
            }
        }
        
        #endregion
    }
}
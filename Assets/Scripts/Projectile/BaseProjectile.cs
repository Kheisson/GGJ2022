using System.Collections;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseProjectile : MonoBehaviour, IProjectile
    {
        #region Fields

        [SerializeField] private ProjectileDetailSO projectileDetails;
        private Rigidbody _projectileRb;

        #endregion

        #region Properties

        public bool IsActive => gameObject.activeInHierarchy;
        public float FireRate => projectileDetails.FireRate;

        #endregion
        
        #region Methods

        private void Awake()
        {
            _projectileRb = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            _projectileRb.velocity = Vector3.up * projectileDetails.ProjectileSpeed;
        }

        private void OnBecameInvisible() => gameObject.SetActive(false);

        public void Fire(Vector2 startingPoint)
        {
            transform.position = startingPoint;
            gameObject.SetActive(true);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using Core;
using Projectile;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields

        [SerializeField] protected EnemySetupSo enemySetupSo;
        private int _health;
        private float _screenBoundryDivider = 1.3f;
        protected bool _disableControl;
        protected List<IProjectile> _projectiles;

        #endregion

        #region Methods

        private void OnEnable()
        {
            _health = enemySetupSo.EnemyHealth;
            _disableControl = true;
            if (_projectiles == null || _projectiles.Count == 0)
                CreateProjectileQueue();
            StartCoroutine(MoveToStartingPosition(new Vector3(0, -GameSettings.ScreenBoundaries.y / _screenBoundryDivider, 0)));
        }

        private void Defeat()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator MoveToStartingPosition(Vector3 startingPosition)
        {
            while (Vector3.Distance(startingPosition, transform.position) > 0.1f) 
            {
                transform.position = Vector3.Lerp(transform.position, startingPosition, Time.deltaTime);
                yield return null;
            }

            transform.position = new Vector3(0, -GameSettings.ScreenBoundaries.y / _screenBoundryDivider, 0);
            _disableControl = false;
            Attack();
        }

        protected virtual void Attack()
        {
            
        }
        
        protected virtual void CreateProjectileQueue()
        {
            
        }

        public void Damage(int damage)
        {
            _health -= damage;
            
            if(_health <= 0)
                Defeat();
        }
        
        #endregion
    }
}
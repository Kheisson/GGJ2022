using System.Collections;
using Projectile;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        private const byte AvailableShots = 3;
        private const string EnemyContainerName = "BasicEnemy Weapon Container";

        private void Update()
        {
            transform.position += Vector3.down * enemySetupSo.EnemySpeed;
        }

        protected override void Attack()
        {
            StartCoroutine(BasicEnemyAttack());
        }

        protected override void CreateProjectileQueue()
        {
            _projectiles = ProjectileFactory.Instance.CreateWeaponQueue(enemySetupSo.EnemyProjectile, AvailableShots, EnemyContainerName);
        }

        private IEnumerator BasicEnemyAttack()
        {
            while (true)
            {
                for (int i = 0; i < AvailableShots; i++)
                {
                    _projectiles[i].Fire(transform.position - (Vector3.up * 6));
                    yield return new WaitForSeconds(0.3f);
                }

                yield return new WaitForSeconds(3f);
            }
        }
    }
}
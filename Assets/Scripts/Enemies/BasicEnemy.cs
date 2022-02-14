using System.Collections;
using Player;
using Projectile;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        private const byte AvailableShots = 3;
        private const string EnemyContainerName = "BasicEnemy Weapon Container";

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
            var playerPos = FindObjectOfType<PlayerControl>();
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
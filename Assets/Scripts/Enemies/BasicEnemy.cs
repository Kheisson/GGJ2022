using System.Collections;
using Player;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        private const byte AvailableShots = 3;

        protected override void Attack()
        {
            print("ATTACK");
            StartCoroutine(BasicEnemyAttack());
        }

        private IEnumerator BasicEnemyAttack()
        {
            var playerPos = FindObjectOfType<PlayerControl>();
            while (true)
            {
                for (int i = 0; i < AvailableShots; i++)
                {
                    var enemyProjectile = Instantiate(enemySetupSo.EnemyProjectile, _spawnProjectilePos.transform);
                    enemyProjectile.transform.LookAt(playerPos.transform.position);
                    enemyProjectile.Fire(transform.position);
                    yield return new WaitForSeconds(0.3f);
                }

                yield return new WaitForSeconds(3f);
            }
        }
    }
}
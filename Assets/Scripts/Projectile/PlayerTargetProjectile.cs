using Player;
using UnityEngine;

namespace Projectile
{
    public class PlayerTargetProjectile : BaseProjectile
    {
        private static PlayerControl _player;
        public override void Fire(Vector2 startingPoint)
        {
            base.Fire(startingPoint);
            
            if (_player == null)
                _player = GameObject.FindObjectOfType<PlayerControl>();
            
            transform.LookAt(_player.transform.position, Vector3.forward);
        }
    }
}
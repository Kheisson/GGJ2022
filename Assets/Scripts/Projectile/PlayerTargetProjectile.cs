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
                _player = FindObjectOfType<PlayerControl>();
            
            transform.LookAt(_player != null ? _player.transform.position : Vector3.zero,
                Vector3.forward);
        }
    }
}
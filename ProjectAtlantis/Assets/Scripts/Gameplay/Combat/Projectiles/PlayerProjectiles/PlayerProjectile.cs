using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class PlayerProjectile : Projectile
    {
        [SerializeField] private float[] scalings;
        
        public override void Initialize(Vector3 newMovementVector, Timing timing)
        {
            base.Initialize(newMovementVector, timing);

            // Rotate in the correct direction.
            var rotation = transform.rotation;
            Quaternion lookRotation = Quaternion.Euler(rotation.eulerAngles.x, GameManager.Instance.Player.ProjectileSpawnPosition.eulerAngles.y, rotation.eulerAngles.z);
            rotation = lookRotation;
            transform.rotation = rotation;

            // For testing.
            switch (timing)
            {
                case Timing.Bad:
                    damage *= scalings[0];
                    break;
                case Timing.Good:
                    damage *= scalings[1];
                    break;
                case Timing.Amazing:
                    damage *= scalings[2];
                    break;
                case Timing.Perfect:
                    damage *= scalings[3];
                    break;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                return;
            
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                enemy.TakeDamage(damage);
            }
            
            base.OnTriggerEnter(other);
        }
    }
}

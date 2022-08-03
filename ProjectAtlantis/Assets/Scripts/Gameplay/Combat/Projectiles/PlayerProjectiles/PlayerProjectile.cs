using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class PlayerProjectile : Projectile
    {
        [SerializeField] protected float[] scalings;
        [SerializeField] private GameObject spawnParticle;
        
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
                    if (spawnParticle != null)
                    {
                        var transform1 = transform;
                        var position = transform1.position;
                       var aObj = Instantiate(spawnParticle, position, Quaternion.identity, transform1);
                       aObj.transform.LookAt(position + newMovementVector); 
                    }
                    
                    break;
                case Timing.Perfect:
                    damage *= scalings[3];

                    if (spawnParticle != null)
                    {
                        var transform1 = transform;
                        var position1 = transform1.position;
                        var pObj = Instantiate(spawnParticle, position1, Quaternion.identity, transform1);
                        pObj.transform.LookAt(position1 + newMovementVector);
                    }
                    break;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") || other.CompareTag("Area Hitbox") || other.CompareTag("PlayerProjectile"))
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

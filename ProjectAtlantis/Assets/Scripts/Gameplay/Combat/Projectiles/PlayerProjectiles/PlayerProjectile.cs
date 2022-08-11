using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class PlayerProjectile : Projectile
    {
        [SerializeField] protected float[] scalings;
        
        [Header("Particle related stuff")]
        [SerializeField] private List<GameObject> spawnParticles;
        [SerializeField] private float spawnParticleUpTime;
        
        public override void Initialize(Vector3 newMovementVector, Timing timing)
        {
            base.Initialize(newMovementVector, timing);

            // Rotate in the correct direction.
            transform.LookAt(transform.position + movementVector);

            GameObject obj = new GameObject();
            
            switch (timing)
            {
                case Timing.Bad:
                    damage *= scalings[0];
                    
                    if (spawnParticles[0] != null)
                    {
                        var transform1 = transform;
                        var position1 = transform1.position;
                        obj = Instantiate(spawnParticles[0], position1, Quaternion.identity, transform1);
                    }
                    break;
                case Timing.Good:
                    damage *= scalings[1];
                    
                    if (spawnParticles[1] != null)
                    {
                        var transform1 = transform;
                        var position1 = transform1.position;
                        obj = Instantiate(spawnParticles[1], position1, Quaternion.identity, transform1);
                    }
                    break;
                case Timing.Amazing:
                    damage *= scalings[2];
                    
                    if (spawnParticles[2] != null)
                    {
                        var transform1 = transform;
                        var position = transform1.position;
                        obj = Instantiate(spawnParticles[2], position, Quaternion.identity, transform1);
                    }
                    
                    break;
                case Timing.Perfect:
                    damage *= scalings[3];
                    
                    if (spawnParticles[3] != null)
                    {
                        var transform1 = transform;
                        var position1 = transform1.position;
                        obj = Instantiate(spawnParticles[3], position1, Quaternion.identity, transform1);
                    }
                    break;
            }
            
            obj.transform.LookAt(transform.position + newMovementVector);
            Destroy(obj, spawnParticleUpTime);
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

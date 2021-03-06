using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Musichopper : PlayerProjectile
    {
        [SerializeField] private bool isHopping;
        
        public override void Initialize(Vector3 newMovementVector, Timing timing)
        {
            base.Initialize(newMovementVector, timing);

            if (timing == Timing.Bad)
                isHopping = true;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Projectile") || other.CompareTag("Player") || other.CompareTag("Area Hitbox") || other.CompareTag("PlayerProjectile"))
                return;

            if (other.CompareTag("Enemy"))
            {
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                movementVector = CalculateNextFlyDirection(enemy);
                enemy.TakeDamage(damage);
                
                // Checks if
                if(isHopping)
                    Destroy(gameObject);
                
                isHopping = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Calculates the direction to fly to next.
        /// </summary>
        /// <param name="lastTarget"> Where the projectile was before the collision. </param>
        /// <returns> Where the projectile should fly to. </returns>
        private Vector3 CalculateNextFlyDirection(BaseEnemy lastTarget)
        {
           var en = GameManager.Instance.EnemyManager.GetClosestEnemy(lastTarget);

           // Failsafe, when no enemies are alive.
           if(en == null)
               return Vector3.down;

           return (en.transform.position - transform.position).normalized * projectileSpeed;
        }
    }
}

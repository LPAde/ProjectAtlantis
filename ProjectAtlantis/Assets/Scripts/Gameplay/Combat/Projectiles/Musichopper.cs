using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles
{
    public class Musichopper : PlayerProjectile
    {
        [SerializeField] private bool isHopping;
        
        public override void Initialize(Timing timing, Vector3 newMovementVector)
        {
            base.Initialize(timing, newMovementVector);

            if (timing == Timing.Bad)
                isHopping = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Projectile"))
                return;

            if (other.CompareTag("Enemy"))
            {
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                enemy.TakeDamage(damage);
                
                // Checks if
                if(!isHopping)
                    Destroy(gameObject);
                
                movementVector = CalculateNextFlyDirection(transform.position);
                isHopping = false;
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
        private Vector3 CalculateNextFlyDirection(Vector3 lastTarget)
        {
           var en = GameManager.Instance.EnemyManager.GetClosestEnemy(lastTarget);
           
           // Failsafe, when no enemies are alive.
           if(en == null)
               return Vector3.down;

           return lastTarget - en.transform.position;
        }
    }
}

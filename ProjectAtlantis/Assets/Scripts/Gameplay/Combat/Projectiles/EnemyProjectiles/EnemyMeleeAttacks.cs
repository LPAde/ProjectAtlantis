using UnityEngine;

namespace Gameplay.Combat.Projectiles.EnemyProjectiles
{
    public class EnemyMeleeAttacks : EnemyProjectile
    {
        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            
            print("hit");
            GameManager.Instance.Player.TakeDamage(damage);
        }
    }
}

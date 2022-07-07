using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class SharkArrow : PlayerProjectile
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Projectile") || other.CompareTag("Player") || other.CompareTag("Area Hitbox"))
                return;
            
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.gameObject.GetComponent<BaseEnemy>();
                enemy.TakeDamage(damage, SpellAbility.Shark);
            }
            
            Destroy(gameObject);
        }
    }
}
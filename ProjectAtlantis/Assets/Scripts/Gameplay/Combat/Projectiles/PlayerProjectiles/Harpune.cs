using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Harpune : PlayerProjectile
    {
        [SerializeField] private Vector3 knockBackVector;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();
            
            en.TakeDamage(damage, knockBackVector, 1);
        }
    }
}
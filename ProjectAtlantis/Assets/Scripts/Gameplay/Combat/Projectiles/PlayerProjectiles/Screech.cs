using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Screech : PlayerProjectile
    {
        [SerializeField] private float stunDuration;
        [SerializeField] private List<BaseEnemy> enemies;

        protected override void FixedUpdate()
        {
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // Checks if the object is an enemy and if it was hit already.
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();
            
            if(enemies.Contains(en))
                return;
            
            en.TakeDamage(damage, stunDuration);
        }
    }
}

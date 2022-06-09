using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Rift : PlayerProjectile
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private float timer;
        
        protected override void FixedUpdate()
        {
            // TODO: Maybe do all that shit through animator.
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Explode();
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // Checks if the object is an enemy and if it was hit already.
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();
            
            if(enemies.Contains(en))
                return;
            
            en.TakeDamage(damage);
        }

        private void Explode()
        {
            // TODO: Code the explosion, can't cause no stack overflow.
        }
    }
}
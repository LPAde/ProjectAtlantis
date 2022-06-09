using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Rift : PlayerProjectile
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private List<BaseEnemy> enemiesInCollider;
        [SerializeField] private float timer;
        
        protected override void FixedUpdate()
        {
            // Maybe do all that shit through animator.
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
            enemiesInCollider.Add(en);
            
            if(enemies.Contains(en))
                return;
            
            en.TakeDamage(damage);
            enemies.Add(en);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();
            enemiesInCollider.Remove(en);
        }

        /// <summary>
        /// Damages every opponent in the collider and destroys itself afterwards. 
        /// </summary>
        private void Explode()
        {
            foreach (var enemy in enemiesInCollider)
            {
                enemy.TakeDamage(damage * 2);
            }
            
            Destroy(gameObject);
        }
    }
}
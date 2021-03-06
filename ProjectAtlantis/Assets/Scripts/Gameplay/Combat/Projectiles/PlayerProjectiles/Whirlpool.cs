using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Whirlpool : PlayerProjectile
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private float maxIntervalTime;
        [SerializeField] private float intervalTime;
        
        protected override void FixedUpdate()
        {
            // Deals damage and knocks back once every few frames.
            intervalTime -= Time.deltaTime;
            
            if(intervalTime > 0)
                return;
            
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    enemies.Remove(enemy);
                
                enemy.TakeDamage(damage, (transform.position - enemy.transform.position) * 50, maxIntervalTime);
                
                if (enemy == null)
                    enemies.Remove(enemy);
            }

            intervalTime = maxIntervalTime;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
               enemies.Add(other.GetComponent<BaseEnemy>()); 
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
                enemies.Remove(other.GetComponent<BaseEnemy>());
        }
    }
}

using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Whirlpool : Projectile
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
                enemy.TakeDamage(damage, enemy.transform.position - transform.position, intervalTime);
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

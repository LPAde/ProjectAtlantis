using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Whirlpool : Projectile
    {
        [SerializeField] private List<BaseEnemy> enemies;

        protected override void FixedUpdate()
        {
            foreach (var enemy in enemies)
            {
                enemy.KnockBack(enemy.transform.position - transform.position);
                enemy.TakeDamage(damage);
            }
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

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

        private void Start()
        {
            // Fixing the rotation and the position of the rift.
            var transform1 = transform;
            transform1.eulerAngles = transform1.eulerAngles + new Vector3(0, 90, 0);
            var position = transform1.position;
            position += position - GameManager.Instance.Player.PlayerController.transform.position;
            transform1.position = position;
        }

        protected override void FixedUpdate()
        {
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

        private void OnDestroy()
        {
            Explode();
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
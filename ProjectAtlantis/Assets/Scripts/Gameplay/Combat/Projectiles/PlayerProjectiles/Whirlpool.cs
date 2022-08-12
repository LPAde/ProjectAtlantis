using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Whirlpool : PlayerProjectile
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private float maxIntervalTime;
        [SerializeField] private float intervalTime;

        public override void Initialize(Vector3 newMovementVector, Timing timing)
        {
            base.Initialize(newMovementVector, timing);

            switch (timing)
            {
                case Timing.Bad:
                    lifeTime *= scalings[0];
                    break;
                case Timing.Good:
                    lifeTime *= scalings[1];
                    break;
                case Timing.Amazing:
                    lifeTime *= scalings[2];
                    break;
                case Timing.Perfect:
                    lifeTime *= scalings[3];
                    break;
            }
        }

        protected override void FixedUpdate()
        {
            // Deals damage and knocks back once every few frames.
            intervalTime -= Time.deltaTime;
            
            if(intervalTime > 0)
                return;
            
            foreach (var enemy in enemies.Where(enemy => enemy != null))
            {
                enemy.TakeDamage(damage, (transform.position - enemy.transform.position), maxIntervalTime);
            }

            intervalTime = maxIntervalTime;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
                return;
            
            var en = other.GetComponent<BaseEnemy>();
            enemies.Add(en); 
            en.TakeDamage(damage, (transform.position - en.transform.position) * 2, maxIntervalTime);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
                enemies.Remove(other.GetComponent<BaseEnemy>());
        }
    }
}
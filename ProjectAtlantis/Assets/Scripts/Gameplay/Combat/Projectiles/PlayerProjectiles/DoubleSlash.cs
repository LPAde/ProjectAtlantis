using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class DoubleSlash : PlayerProjectile
    {
        [SerializeField] private Collider formerCollider;
        [SerializeField] private Collider newCollider;
        [SerializeField] private bool isSecondHitBox;
        [SerializeField] private List<Collider> hitEnemies;
        
        protected override void Update()
        {
            if(lifeTime < .1f)
                ChangeHitBox();
            
            base.Update();
        }

        protected override void FixedUpdate()
        {
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Enemy"))
                return;

            if(hitEnemies.Contains(other))
                return;
            
            hitEnemies.Add(other);
            var en = other.GetComponent<BaseEnemy>();

            if (!isSecondHitBox)
            {
                // Knocks opponent to the desired position.
                en.TakeDamage(damage, (GameManager.Instance.Player.ProjectileSpawnPosition.position - en.transform.position) * 2, lifeTime - .1f);
            }
            else
            {
                en.TakeDamage(damage*2);
            }
        }

        private void ChangeHitBox()
        {
            formerCollider.enabled = false;
            isSecondHitBox = true;
            newCollider.enabled = true;
            hitEnemies.Clear();
        }
    }
}
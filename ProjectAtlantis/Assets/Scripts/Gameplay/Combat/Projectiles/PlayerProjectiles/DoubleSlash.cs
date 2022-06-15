using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class DoubleSlash : PlayerProjectile
    {
        [SerializeField] private Collider formerCollider;
        [SerializeField] private Collider newCollider;
        [SerializeField] private bool isSecondHitBox;
        
        protected override void FixedUpdate()
        {
            if(lifeTime < .5f)
                ChangeHitBox();
            
            base.FixedUpdate();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();

            if (!isSecondHitBox)
            {
                // Knocks opponent to the desired position.
                en.TakeDamage(damage, GameManager.Instance.Player.ProjectileSpawnPosition.position * 3 - en.transform.position);
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
        }
    }
}
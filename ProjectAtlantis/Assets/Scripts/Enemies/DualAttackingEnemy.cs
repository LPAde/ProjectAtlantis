using Gameplay.Combat.Projectiles.EnemyProjectiles;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class DualAttackingEnemy : AttackingMeleeEnemy
    {
        [SerializeField] private float meleeDistance;

        public override void Attack()
        {
            if (stats.AttackCooldown > 0)
            {
                stats.AttackCooldown -= Time.deltaTime;
                return;
            }
            
            if ((transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude <
                meleeDistance)
            {
                // TODO: Play Melee Animation
                anim.SetTrigger("MeleeAttack");
            }
            else
            {
                //TODO: Play Ranged Animation
                anim.SetTrigger("RangedAttack");
            }

            stats.AttackCooldown = stats.AttackMaxCooldown;
            source.Play();
        }

        public void StartMeleeAttack()
        {
            IsInAnimation = true;
            if(DidHit)
                return;
            
            if (meleeDistance > (attack.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude)
            {
                GameManager.Instance.Player.TakeDamage(stats.Strength);
                DidHit = true;
            }
        }

        public void StartRangedAttack()
        {
            IsInAnimation = true;
            var position = projectileSpawnPosition.position;
            var projectile =
                Instantiate(attack, position, quaternion.identity,
                    GameManager.Instance.transform).GetComponent<EnemyProjectile>();
            
            projectile.Initialize(transform.forward);
            
        }
    }
}

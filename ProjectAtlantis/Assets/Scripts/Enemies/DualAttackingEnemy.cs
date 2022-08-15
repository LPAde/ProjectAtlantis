using System;
using Gameplay.Combat.Projectiles.EnemyProjectiles;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    public class DualAttackingEnemy : AttackingMeleeEnemy
    {
        [SerializeField] private float meleeDistanceSquared;

        public override void Attack()
        {
            if (stats.AttackCooldown > 0)
            {
                stats.AttackCooldown -= Time.deltaTime;
                return;
            }
            
            // Check Distance to player to decide on melee or ranged animation
            anim.SetTrigger((transform.position - GameManager.Instance.Player.PlayerController.transform.position)
                            .sqrMagnitude <
                            meleeDistanceSquared
                ? "MeleeAttack"
                : "RangedAttack");

            stats.AttackCooldown = stats.AttackMaxCooldown;
            source.Play();
        }

        public void StartMeleeAttack()
        {
            IsInAnimation = true;
            if(DidHit)
                return;
            
            if (meleeDistanceSquared > (projectileSpawnPosition.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude)
            {
                GameManager.Instance.Player.TakeDamage(stats.Strength * 2);
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
            
            projectile.Initialize(transform.forward, stats.Strength);
        }
    }
}

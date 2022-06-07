using Gameplay.Combat.Projectiles.EnemyProjectiles;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class AttackingEnemy : BaseEnemy
    {
        [Header("Attack related values")]
        [SerializeField] private GameObject attack;
        [SerializeField] private Transform projectileSpawnPosition;
        private bool _mayAttack;
        
        public Transform ProjectileSpawnPosition => projectileSpawnPosition;

        protected override void OnEnable()
        {
            GameManager.Instance.RhythmManager.HitPerfect += MayAttack;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            GameManager.Instance.RhythmManager.HitPerfect -= MayAttack;
            base.OnDisable();
        }
        
        /// <summary>
        /// Checks cooldown and attacks when it should.
        /// </summary>
        public virtual void Attack()
        {
            if (stats.AttackCooldown > 0)
            {
                stats.AttackCooldown -= Time.deltaTime;
            }
            else
            {
                if(!_mayAttack)
                    return;

                var position = projectileSpawnPosition.position;
                var projectile =
                    Instantiate(attack, position, quaternion.identity,
                        GameManager.Instance.transform).GetComponent<EnemyProjectile>();
                projectile.Initialize(GameManager.Instance.Player.PlayerController.transform.position - position);

                stats.AttackCooldown = stats.AttackMaxCooldown;
            }
        }
        
        private void MayAttack()
        {
            if(stats.AttackCooldown > 0)
                return;
            
            _mayAttack = true;
        }
    }
}

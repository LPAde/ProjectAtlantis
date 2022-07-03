using Enemies.AI.FiniteStateMachines;
using Gameplay.Combat.Projectiles.EnemyProjectiles;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class AttackingEnemy : BaseEnemy
    {
        [Header("Attack related values")]
        [SerializeField] protected GameObject attack;
        [SerializeField] private Transform projectileSpawnPosition;
        protected bool mayAttack;
        
        
        public AttackerFsm FiniteAttackerStateMachine { get; private set; }
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

        protected override void Start()
        {
            FiniteAttackerStateMachine = new AttackerFsm(this);
            
            FiniteAttackerStateMachine.Initialize(FiniteAttackerStateMachine.IdleState);
        }

        protected override void Update()
        {
            FiniteAttackerStateMachine.Update();
        }

        public override void WalkToPlayer()
        {
            base.WalkToPlayer();

            float distance = (FiniteAttackerStateMachine.Owner.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude;

            if(distance < stats.AttackRange*stats.AttackRange)
                FiniteAttackerStateMachine.Transition(FiniteAttackerStateMachine.FightState);
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
                if(!mayAttack)
                    return;

                var position = projectileSpawnPosition.position;
                var projectile =
                    Instantiate(attack, position, quaternion.identity,
                        GameManager.Instance.transform).GetComponent<EnemyProjectile>();
                projectile.Initialize(GameManager.Instance.Player.PlayerController.transform.position - position);

                stats.AttackCooldown = stats.AttackMaxCooldown;

                mayAttack = false;
            }
        }

        protected override void Stun(float duration)
        {
            stunTime = duration;
            FiniteAttackerStateMachine.Transition(FiniteAttackerStateMachine.StunState);
        }

        /// <summary>
        /// Allows the attack when the timing was perfect.
        /// </summary>
        private void MayAttack()
        {
            if(stats.AttackCooldown > 0)
                return;
            
            mayAttack = true;
        }
    }
}

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
        [SerializeField] protected AudioSource source;
        
        public bool IsInAnimation { get; internal set; }
        public AttackerFsm FiniteAttackerStateMachine { get; private set; }
        public Transform ProjectileSpawnPosition => projectileSpawnPosition;

        protected override void Start()
        {
            FiniteAttackerStateMachine = new AttackerFsm(this);
            
            FiniteAttackerStateMachine.Initialize(FiniteAttackerStateMachine.IdleState);
        }

        public override void EnemyUpdate()
        {
            FiniteAttackerStateMachine.Update();
        }

        public override void WalkToPlayer()
        {
            var target = GameManager.Instance.Player.PlayerController.transform.position;
            
            if((target-transform.position).magnitude < stats.AttackRange)
            {
                Stop();
            }
            
            base.WalkToPlayer();

            float distance = (FiniteAttackerStateMachine.Owner.transform.position - target).sqrMagnitude;
            
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
                stats.AttackCooldown = stats.AttackMaxCooldown;
                anim.SetTrigger("Attack");
            }
        }
        
        public virtual void StartAttack()
        {
            IsInAnimation = true;
            var position = projectileSpawnPosition.position;
            var projectile =
                Instantiate(attack, position, quaternion.identity,
                    GameManager.Instance.transform).GetComponent<EnemyProjectile>();
            
            projectile.Initialize(transform.forward);
            
            source.Play();
        }

        public virtual void EndAttack()
        {
            IsInAnimation = false;
        }

        protected override void Stun(float duration)
        {
            stunTime = duration;
            FiniteAttackerStateMachine.Transition(FiniteAttackerStateMachine.StunState);
        }
    }
}

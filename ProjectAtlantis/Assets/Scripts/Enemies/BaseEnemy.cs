using System;
using Combat;
using Enemies.AI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyStats stats;
        
        [SerializeField] private float difficultyModifier = 1;
        [SerializeField] private GameObject attack;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private NavMeshAgent agent;
        
        public FiniteStateMachine FiniteStateMachine { get; private set; }

        public EnemyStats Stats => stats;

        private void Start()
        {
            FiniteStateMachine = new FiniteStateMachine(this);
            
            FiniteStateMachine.Initialize(FiniteStateMachine.IdleState);
        }

        private void Update()
        {
            FiniteStateMachine.Update();
        }

        /// <summary>
        /// Deals damage to the player.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        public void TakeDamage(float damage)
        {
            float takenDamage = damage - stats.Defense;
            
            if(takenDamage <= 0)
                return;
            
            stats.Health -= takenDamage * difficultyModifier;
            
            if(stats.Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Walks to the player through the navmesh.
        /// </summary>
        public virtual void WalkToPlayer()
        {
            agent.isStopped = false;
            agent.SetDestination(GameManager.Instance.Player.PlayerController.transform.position);
        }

        /// <summary>
        /// Stops the enemy.
        /// </summary>
        public void Stop()
        {
            agent.isStopped = true;
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
                if(GameManager.Instance.RhythmManager.CheckTiming() != Timing.Perfect)
                    return;
                
                var projectile =
                    Instantiate(attack, projectileSpawnPosition.position, quaternion.identity,
                        GameManager.Instance.transform).GetComponent<EnemyProjectile>();
                projectile.Initialize(GameManager.Instance.Player.PlayerController.transform.position - projectileSpawnPosition.position);

                stats.AttackCooldown = stats.AttackMaxCooldown;
            }
        }
    }

    [Serializable]
    public struct EnemyStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float defense;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackMaxCooldown;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float triggerRange;

        public float MAXHealth
        {
            get => maxHealth;
            internal set => maxHealth = value;
        }

        public float Health
        {
            get => health;
            internal set
            {
                health = value;

                if (health > MAXHealth) 
                    health = MAXHealth;
            }
        }

        public float Strength
        {
            get => strength;
            internal set => strength = value;
        }

        public float Defense
        {
            get => defense;
            internal set => defense = value;
        }

        public float AttackRange
        {
            get => attackRange;
            internal set => attackRange = value;
        }
        
        public float AttackMaxCooldown
        {
            get => attackMaxCooldown;
            internal set => attackMaxCooldown = value;
        }
        
        public float AttackCooldown
        {
            get => attackCooldown;
            internal set => attackCooldown = value;
        }
        
        public float TriggerRange
        {
            get => triggerRange;
            internal set => triggerRange = value;
        }
    } 
}

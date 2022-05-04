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
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float desiredSeparation;
        [SerializeField] private float maxForce;
        
        private bool mayAttack;
        
        public FiniteStateMachine FiniteStateMachine { get; private set; }

        public EnemyStats Stats => stats;

        private void OnEnable()
        {
            GameManager.Instance.RhythmManager.HitPerfect += MayAttack;
            GameManager.Instance.EnemyManager.AddEnemy(this);
        }

        private void Start()
        {
            FiniteStateMachine = new FiniteStateMachine(this);
            
            FiniteStateMachine.Initialize(FiniteStateMachine.IdleState);
        }

        private void Update()
        {
            FiniteStateMachine.Update();
        }

        private void OnDisable()
        {
            GameManager.Instance.RhythmManager.HitPerfect -= MayAttack;
            GameManager.Instance.EnemyManager.RemoveEnemy(this);
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
            //rb.AddForce(CalculateSeparation());
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
                if(!mayAttack)
                    return;
                
                var projectile =
                    Instantiate(attack, projectileSpawnPosition.position, quaternion.identity,
                        GameManager.Instance.transform).GetComponent<EnemyProjectile>();
                projectile.Initialize(GameManager.Instance.Player.PlayerController.transform.position - projectileSpawnPosition.position);

                stats.AttackCooldown = stats.AttackMaxCooldown;
            }
        }
        
        private void MayAttack()
        {
            mayAttack = true;
        }

        private Vector3 CalculateSeparation()
        {
            Vector3 totalSeparation = Vector3.zero;
            int neighbourCount = 0;

            var enManager = GameManager.Instance.EnemyManager;
            for (int i = 0; i < enManager.Enemies.Count; i++)
            {
                var en = enManager.Enemies[i];
                Vector3 separation = transform.position - en.transform.position;
                float distance = separation.magnitude;

                if (distance > 0 && distance < desiredSeparation)
                {
                    separation.Normalize();
                    separation /= distance;
                    totalSeparation += separation;
                    neighbourCount++;
                }
            }

            if (neighbourCount > 0)
            {
                Vector3 averageSeparation = totalSeparation / neighbourCount;
                averageSeparation = averageSeparation.normalized * stats.Speed;
                Vector3 separationForce = averageSeparation - rb.velocity;

                if (separationForce.magnitude > maxForce)
                    separationForce = separationForce.normalized * maxForce;

                return separationForce;
            }
            
            return Vector3.zero;
        }
    }

    [Serializable]
    public struct EnemyStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float defense;
        [SerializeField] private float speed;
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
        
        public float Speed
        {
            get => speed;
            internal set => speed = value;
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

using System;
using Enemies.AI.FiniteStateMachines;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class BaseEnemy : Character
    {
        [SerializeField] protected EnemyStats stats;
        
        [SerializeField] private float difficultyModifier = 1;
        [SerializeField] private float combatScore;
        [SerializeField] protected float stunTime;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rb;

        [Header("Flocking related values")]
        [SerializeField] private float desiredSeparation;
        [SerializeField] private float maxForce;

        private bool wasSharked;
        
        public FiniteStateMachine FiniteStateMachine { get; private set; }

        public EnemyStats Stats => stats;

        public float CombatScore => combatScore;
        
        
        protected virtual void OnEnable()
        {
            GameManager.Instance.EnemyManager.AddEnemy(this);
            
            if(GameManager.Instance.ArenaManager.IsInArena)
                GameManager.Instance.ArenaManager.AddArenaEnemy(this);
        }

        protected virtual void OnDisable()
        {
            
            GameManager.Instance.EnemyManager.RemoveEnemy(this);
            
            if(GameManager.Instance.ArenaManager.IsInArena)
                GameManager.Instance.ArenaManager.RemoveArenaEnemy(this);
        }

        protected virtual void Start()
        {
            FiniteStateMachine = new FiniteStateMachine(this);
            
            FiniteStateMachine.Initialize(FiniteStateMachine.IdleState);
        }

        protected virtual void Update()
        {
            FiniteStateMachine.Update();
        }

        /// <summary>
        /// Deals damage to the enemy.
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
        /// Deals damage and applies special spell effect.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        /// <param name="ability"> The special effect of the spell. </param>
        public void TakeDamage(float damage, SpellAbility ability)
        {
            switch (ability)
            {
                case SpellAbility.Shark:

                    if (!wasSharked)
                    {
                        wasSharked = true;
                        TakeDamage(damage);
                    }
                    else
                    {
                        damage *= 3;
                        TakeDamage(damage);
                    }
                    
                    break;
            }
        }

        /// <summary>
        /// Deals damage to the enemy and knocks it back.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        /// <param name="knockBack"> How much the enemy is knocked back. </param>
        public void TakeDamage(float damage, Vector3 knockBack)
        {
            TakeDamage(damage);
            KnockBack(knockBack);
        }

        /// <summary>
        /// Deals damage to the enemy and stuns it.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        /// <param name="duration"> How long the enemy is stunned for. </param>
        public void TakeDamage(float damage, float duration)
        {
            TakeDamage(damage);
            Stun(stunTime = duration);
        }

        /// <summary>
        /// Walks to the player through the navmesh.
        /// </summary>
        public virtual void WalkToPlayer()
        {
            agent.isStopped = false;
            agent.SetDestination(GameManager.Instance.Player.PlayerController.transform.position + CalculateSeparation());
        }

        /// <summary>
        /// Stops the enemy.
        /// </summary>
        public void Stop()
        {
            agent.isStopped = true;
        }
        
        /// <summary>
        /// Stuns the enemy for a set amount of time.
        /// </summary>
        public float EndureStun()
        {
            stunTime -= Time.deltaTime;
            return stunTime;
        }
        
        /// <summary>
        /// Knocks the enemy in a direction once.
        /// </summary>
        /// <param name="knockBackVector"> The direction you want them to be knocked to. </param>
        private void KnockBack(Vector3 knockBackVector)
        {
            agent.updatePosition = false;
            rb.AddForce(knockBackVector);
            agent.updatePosition = true;
        }

        protected virtual void Stun(float duration)
        {
            stunTime = duration;
            FiniteStateMachine.Transition(FiniteStateMachine.StunState);
        }

        /// <summary>
        /// Does the flocking behaviour.
        /// </summary>
        /// <returns> How much the Enemy has to change their movement. </returns>
        private Vector3 CalculateSeparation()
        {
            // Setup.
            Vector3 totalSeparation = Vector3.zero;
            int neighbourCount = 0;
            var enManager = GameManager.Instance.EnemyManager;
            
            // Checks how many other enemies are close.
            foreach (var en in enManager.Enemies)
            {
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

            // Sets the separation vector.
            if (neighbourCount > 0)
            {
                Vector3 averageSeparation = totalSeparation / neighbourCount;
                averageSeparation = averageSeparation.normalized * stats.Speed;
                Vector3 separationVector = averageSeparation - agent.velocity;

                if (separationVector.magnitude > maxForce)
                    separationVector = separationVector.normalized * maxForce;

                return separationVector;
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

    public enum SpellAbility
    {
        Shark,
    }
}
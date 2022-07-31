using System;
using System.Collections.Generic;
using Enemies.AI.FiniteStateMachines;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
        [SerializeField] protected Animator anim;

        [Header("Drop related values")] 
        [SerializeField] private List<GameObject> droppableItems;
        [SerializeField] private List<int> spawnChances;
        [SerializeField] private int lastPossibleHealDrop;
        
        private bool _wasSharked;
        
        public FiniteStateMachine FiniteStateMachine { get; private set; }
        public EnemyStats Stats => stats;
        public float CombatScore => combatScore;
        public bool IsArenaEnemy { get; private set; }
        
        protected virtual void OnEnable()
        {
            GameManager.Instance.EnemyManager.AddEnemy(this);
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

        public void MakeArenaEnemy()
        {
            IsArenaEnemy = true;
        }
        
        /// <summary>
        /// Deals damage to the enemy.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        public void TakeDamage(float damage)
        {
            float takenDamage = damage - stats.Defense;
            
            // Ensure at least one damage is dealt.
            if(takenDamage <= 0)
                takenDamage = 1;
            
            stats.Health -= takenDamage * difficultyModifier;
            
            if(stats.Health <= 0)
            {
                Die();
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

                    if (!_wasSharked)
                    {
                        _wasSharked = true;
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
        /// <param name="knockBackTime"> How long the knock back will last. </param>
        public void TakeDamage(float damage, Vector3 knockBack, float knockBackTime)
        {
            KnockBack(knockBack, knockBackTime);
            TakeDamage(damage);
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
            var target = GameManager.Instance.Player.PlayerController.transform.position;
            
            if((target-transform.position).magnitude < 3)
            {
                Stop();
                return;
            }
            
            print((target-transform.position).magnitude);
            
            agent.SetDestination(target);
        }

        /// <summary>
        /// Stops the enemy.
        /// </summary>
        public void Stop()
        {
            agent.isStopped = true;
        }

        public void ResetVelocity()
        {
            rb.velocity = Vector3.zero;
        }
        
        /// <summary>
        /// Stuns the enemy for a set amount of time.
        /// </summary>
        public float EndureStun()
        {
            stunTime -= Time.deltaTime;

            if (stunTime <= 0)
                rb.isKinematic = true;
            
            return stunTime;
        }

        /// <summary>
        /// Sets the bool of the enemies animator.
        /// </summary>
        /// <param name="animBool"></param>
        /// <param name="boolState"></param>
        public void SetAnimBool(string animBool, bool boolState)
        {
            anim.SetBool(animBool, boolState);
        }
        
        private void Die()
        {
            if (spawnChances.Count == 0)
            {
                Destroy(gameObject);
                return;
            }

            int random = Random.Range(0, 1000);
            float playerHeight = GameManager.Instance.Player.PlayerController.transform.position.y;
            var heightChecks = GameManager.Instance.ArenaManager.HeightChecks;
            
            if (playerHeight > heightChecks[0])
            {
                // Doubling all heal Items spawn chances.
                for (int i = 0; i < lastPossibleHealDrop; i++)
                {
                    spawnChances[i] *= 2;
                }
            }
            else if(playerHeight > heightChecks[1])
            {
                for (int i = 0; i < droppableItems.Count; i++)
                {
                    spawnChances[i] = (int)(spawnChances[i] * 1.5f);
                }
            }
            else
            {
                for (int i = lastPossibleHealDrop; i < droppableItems.Count; i++)
                {
                    spawnChances[i] *= 2;
                }
            }
            

            for (int i = droppableItems.Count - 1; i > -1; i--)
            {
                if (random < spawnChances[i])
                {
                    Instantiate(droppableItems[i], transform.position, Quaternion.identity, GameManager.Instance.transform);
                    break;
                }
            }
            
            Destroy(gameObject);
        }

        /// <summary>
        /// Knocks the enemy in a direction once.
        /// </summary>
        /// <param name="knockBackVector"> The direction you want them to be knocked to. </param>
        /// <param name="knockbackTime"> How long they get knocked back. </param>
        private void KnockBack(Vector3 knockBackVector, float knockbackTime)
        {
            rb.isKinematic = false;
            rb.AddForce(knockBackVector);
            Stun(stunTime = knockbackTime);
        }

        protected virtual void Stun(float duration)
        {
            stunTime = duration;
            FiniteStateMachine.Transition(FiniteStateMachine.StunState);
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
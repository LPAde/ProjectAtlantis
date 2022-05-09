using System;
using Combat.Spells;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : Character
    {
        #region Private Fields

        [SerializeField] private PlayerController playerController;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerStats stats;
        
        [Header("Attack Related Stuff")]
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private CombatSpell[] combatSpells;
        [SerializeField] private MovementSpell movementSpell;

        // In case we want to add some simple difficulty modes.
        [SerializeField] private float difficultyModifier = 1;
        
        #endregion
        
        public Action OnPlayerDeath;

        #region Properties

        public PlayerController PlayerController => playerController;
        public CharacterController CharacterController => characterController;
        public PlayerStats PlayerStats => stats;
        public CombatSpell[] CombatSpells => combatSpells;
        public MovementSpell MovementSpell => movementSpell;
        public Transform ProjectileSpawnPosition => projectileSpawnPosition;

        #endregion

        private void Awake()
        {
            foreach (var spell in combatSpells)
            {
                spell.SetOwner(this);
            }
            
            movementSpell.SetOwner(this);
        }

        private void LateUpdate()
        {
            foreach (var spell in combatSpells)
            {
                spell.TickDownCooldown();
            }
            
            movementSpell.TickDownCooldown();
        }

        /// <summary>
        /// Deals damage to the player.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        public void TakeDamage(float damage)
        {
            print(damage);
            float takenDamage = damage - stats.Defense;
            
            if(takenDamage <= 0)
                return;
            
            stats.Health -= takenDamage * difficultyModifier;
            
            GameManager.Instance.HudManager.UpdateHealth(stats.Health);
            
            if(stats.Health <= 0)
                OnPlayerDeath.Invoke();
        }

        /// <summary>
        /// Heals the player.
        /// </summary>
        /// <param name="value"> How much should be healed. </param>
        public void Heal(float value)
        {
            stats.Health += value * difficultyModifier;
            
            GameManager.Instance.HudManager.UpdateHealth(stats.Health);
            
            if (stats.Health > stats.MAXHealth)
                stats.Health = stats.MAXHealth;
        }
    }


    [Serializable]
    public struct PlayerStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float defense;
        [SerializeField] private float speed;

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

        public void UpdateStats(PlayerStats addedStats)
        {
            maxHealth += addedStats.maxHealth;
            health += addedStats.maxHealth;
            strength += addedStats.strength;
            defense += addedStats.defense;
        }
    }
}
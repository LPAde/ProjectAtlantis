using System;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyStats stats;
        
        [SerializeField] private float difficultyModifier = 1;
        [SerializeField] private GameObject attack;
        
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
    }

    [Serializable]
    internal struct EnemyStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float defense;

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
    } 
}

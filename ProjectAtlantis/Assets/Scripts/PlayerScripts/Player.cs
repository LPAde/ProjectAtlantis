using System;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private PlayerStats stats;
        
        [Header("Attack Related Stuff")]
        [SerializeField] private GameObject playerAttack;
        [SerializeField] private Transform projectileSpawnPosition;

        #endregion
        

        public Action OnPlayerDeath;
        
        public GameObject PlayerAttack => playerAttack;
        public Transform ProjectileSpawnPosition => projectileSpawnPosition;

        /// <summary>
        /// Deals damage to the player.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        public void TakeDamage(float damage)
        {
            stats.Health -= damage - stats.Defense;
            
            if(stats.Health <= 0)
                OnPlayerDeath.Invoke();
        }
    }


    [Serializable]
    public struct PlayerStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float defense;

        internal float MAXHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        internal float Health
        {
            get => health;
            set
            {
                health = value;

                if (health > MAXHealth) 
                    health = MAXHealth;
            }
        }

        internal float Strength
        {
            get => strength;
            set => strength = value;
        }

        internal float Defense
        {
            get => defense;
            set => defense = value;
        }

    }
}

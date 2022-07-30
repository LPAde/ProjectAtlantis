using System;
using System.Collections.Generic;
using Gameplay.Combat.Spells;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerScripts
{
    public class Player : Character
    {
        #region Private Fields

        [SerializeField] private PlayerController playerController;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private AudioSource loopingAudioSource;
        [SerializeField] private AudioSource oneTimeAudioSource;
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private Animator anim;
        
        [Header("Attack Related Stuff")]
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private List<Transform> bubblePositions;
        [SerializeField] private CombatSpell[] combatSpells;
        [SerializeField] private MovementSpell movementSpell;

        // In case we want to add some simple difficulty modes.
        [SerializeField] private float difficultyModifier = 1;
        
        #endregion
        
        public Action OnPlayerDeath;

        #region Properties

        public PlayerController PlayerController => playerController;
        public CharacterController CharacterController => characterController;
        public AudioSource LoopingAudioSource => loopingAudioSource;
        public PlayerStats PlayerStats => stats;
        public Animator Anim => anim;
        public Transform ProjectileSpawnPosition => projectileSpawnPosition;
        public List<Transform> BubblePositions => bubblePositions;
        public CombatSpell[] CombatSpells => combatSpells;
        public MovementSpell MovementSpell => movementSpell;

        #endregion

        private void Awake()
        {
            GameManager.Instance.Load += Load;
            GameManager.Instance.Save += Save;
            OnPlayerDeath += EasyDeath;
            
            foreach (var spell in combatSpells)
            {
                spell.SetOwner(this);
            }
            
            movementSpell.SetOwner(this);
            
            GameManager.Instance.HudManager.UpdateSkills(combatSpells, movementSpell);
        }

        private void LateUpdate()
        {
            // Reducing spell cooldown.
            for (var index = 0; index < combatSpells.Length; index++)
            {
                GameManager.Instance.HudManager.CooldownSkill(combatSpells[index].TickDownCooldown(), index);
            }

            GameManager.Instance.HudManager.CooldownSkill(movementSpell.TickDownCooldown(), 3);
            
            // Health regeneration. 
            Heal(stats.HealthRegen * Time.deltaTime);
        }

        /// <summary>
        /// Deals damage to the player.
        /// </summary>
        /// <param name="damage"> How much damage is dealt. </param>
        public void TakeDamage(float damage)
        {
            float takenDamage = damage - stats.Defense;
            
            // Ensure at least one damage is dealt.
            if(takenDamage <= 0)
                takenDamage = 1;
            
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
        
        /// <summary>
        /// Upgrades the stats of the player.
        /// </summary>
        /// <param name="addedStats"> The stats to be added. </param>
        public void UpgradeStats(PlayerStats addedStats)
        {
            stats.MAXHealth += addedStats.MAXHealth;
            stats.Health += addedStats.MAXHealth;
            stats.HealthRegen += addedStats.HealthRegen;
            stats.Strength += addedStats.Strength;
            stats.Defense += addedStats.Defense;
            stats.Speed += addedStats.Speed;
            
            GameManager.Instance.HudManager.UpdateHealth(stats.Health, stats.MAXHealth);
            GameManager.Instance.HudManager.UpdateStats(stats);
            
            // Always saving after Statupgrade.
            GameManager.Instance.Save.Invoke();
        }
        
        /// <summary>
        /// Plays a desired sound once.
        /// </summary>
        /// <param name="sound"> The desired sound. </param>
        public void PlayOneTimeSound(PlayerSounds sound)
        {
            oneTimeAudioSource.Stop();
            oneTimeAudioSource.clip = audioClips[(int) sound];
            oneTimeAudioSource.Play();
        }

        /// <summary>
        /// Plays a desired sound in a loop.
        /// </summary>
        /// <param name="sound"> The desired sound. </param>
        public void PlayLoopingSound(PlayerSounds sound)
        {
            loopingAudioSource.Stop();
            loopingAudioSource.clip = audioClips[(int) sound];
            loopingAudioSource.Play();
        }

        /// <summary>
        /// Stops the looping sound and plays the idle instead.
        /// </summary>
        public void StopLoopingSound()
        {
            loopingAudioSource.Stop();
            loopingAudioSource.clip = audioClips[(int) PlayerSounds.Idle];
            loopingAudioSource.Play();
        }
        
        /// <summary>
        /// Temporary method for testing.
        /// </summary>
        private void EasyDeath()
        {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Uses the external save system to load and overwrite the players data.
        /// </summary>
        private void Load()
        {
            // Stats.
            string statsString = SaveSystem.GetString("PlayerStats");
            
            if(string.IsNullOrEmpty(statsString))
                return;
            
            var allStats = statsString.Split("*");

            stats.MAXHealth = float.Parse(allStats[0]);
            stats.Health = float.Parse(allStats[0]);
            stats.HealthRegen = float.Parse(allStats[2]);
            stats.Strength = float.Parse(allStats[3]);
            stats.Defense = float.Parse(allStats[4]);
            stats.Speed = float.Parse(allStats[5]);

            // Spells.
            string spellString = SaveSystem.GetString("PlayerSpells");
            var idStrings = spellString.Split("*");
            
            for (int i = 0; i < combatSpells.Length; i++)
            {
                combatSpells[i] = (CombatSpell)GameManager.Instance.SpellManager.GetSpell(int.Parse(idStrings[i]));
            }

            movementSpell = (MovementSpell) GameManager.Instance.SpellManager.GetSpell(int.Parse(idStrings[3]));
            
            GameManager.Instance.HudManager.UpdateSkills(combatSpells, movementSpell);
        }

        /// <summary>
        /// Uses the external save system to save the players data.
        /// </summary>
        private void Save()
        {
            SaveSystem.SetString
                ("PlayerStats", string.Concat(stats.MAXHealth, "*", stats.Health, "*",stats.HealthRegen, "*", stats.Strength, "*",stats.Defense, "*", stats.Speed));
            
            SaveSystem.SetString
                ("PlayerSpells", string.Concat(GameManager.Instance.SpellManager.GetSpellID(combatSpells[0]), "*",
                GameManager.Instance.SpellManager.GetSpellID(combatSpells[1]), "*",GameManager.Instance.SpellManager.GetSpellID(combatSpells[2])
                , "*",GameManager.Instance.SpellManager.GetSpellID(movementSpell), "*"));
        }
    }

    [Serializable]
    public struct PlayerStats
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float health;
        [SerializeField] private float healthRegen;
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

        public float HealthRegen
        {
            get => healthRegen;
            internal set => healthRegen = value;
            
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
    }

    public enum PlayerSounds
    {
        Swimming = 0,
        StartSwimming = 1,
        Idle = 2,
        Attack = 3, 
        Attack2 = 4,
    }
}
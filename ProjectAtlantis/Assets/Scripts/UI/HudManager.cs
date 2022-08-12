using System.Collections.Generic;
using Gameplay.Combat.Spells;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudManager : MonoBehaviour
    {
        [Header("Gameplay Info")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private List<Button> skillIndicators;
        [SerializeField] private List<TextMeshProUGUI> skillCooldowns;
        [SerializeField] private TextMeshProUGUI waveCountText;
        [SerializeField] private Animator anim;
        
        [Header("Stat-screen")] 
        [SerializeField] private GameObject statScreen;
        [SerializeField] private List<TextMeshProUGUI> statsText;

        private void Start()
        {
            healthBar.maxValue = GameManager.Instance.Player.PlayerStats.MAXHealth;
            healthBar.value = GameManager.Instance.Player.PlayerStats.Health;
            
            UpdateStats(GameManager.Instance.Player.PlayerStats);
        }

        private void Update()
        {
            statScreen.SetActive(Input.GetKey(KeyCode.Tab));
        }

        public void HitAnimation()
        {
            anim.SetTrigger("Hit");
        }
        
        /// <summary>
        /// Updates the health bar.
        /// </summary>
        /// <param name="newCurrentHealth"> The new health bar value. </param>
        public void UpdateHealth(float newCurrentHealth)
        {
            healthBar.value = newCurrentHealth;
        }

        /// <summary>
        /// Updates the health bar with the new max health.
        /// </summary>
        /// <param name="newCurrentHealth"> The new health bar value. </param>
        /// <param name="newMaxHealth"> The new health bar max value. </param>
        public void UpdateHealth(float newCurrentHealth, float newMaxHealth)
        {
            healthBar.maxValue = newMaxHealth;
            healthBar.value = newCurrentHealth;
        }

        /// <summary>
        /// Updates the Icons of the spells.
        /// </summary>
        /// <param name="combatSpells"> The combat spells. </param>
        /// <param name="movementSpell"> The movement spell. </param>
        public void UpdateSkills(List<CombatSpell> combatSpells, MovementSpell movementSpell)
        {
            for (int i = 0; i < combatSpells.Count; i++)
            {
                skillIndicators[i].image.sprite = combatSpells[i].SpellSprite;
            }

            skillIndicators[3].image.sprite = movementSpell.SpellSprite;
        }

        /// <summary>
        /// Updates the current wave count.
        /// </summary>
        /// <param name="waveCount"> The wave the player is at. </param>
        public void UpdateWaveCount(int waveCount)
        {
            waveCountText.text = string.Concat("Current Wave: ", waveCount.ToString());
        }

        /// <summary>
        /// Updates the stats at the stats screen.
        /// </summary>
        /// <param name="stats"> The players stats. </param>
        public void UpdateStats(PlayerStats stats)
        {
            statsText[0].text = stats.MAXHealth.ToString("0"); 
            statsText[1].text = stats.HealthRegen.ToString("0.0");
            statsText[2].text = stats.Strength.ToString("0");
            statsText[3].text = stats.Defense.ToString("0");
            statsText[4].text = stats.Speed.ToString("0");
            
            float cdr = stats.CoolDownReduction * 100;
            statsText[5].text = cdr.ToString("0.00");
        }

        /// <summary>
        /// Shows the player that a skill has been activated.
        /// </summary>
        /// <param name="index"> Which skill has been used. </param>
        public void UseSkill(int index)
        {
            skillIndicators[index].interactable = false;
            skillCooldowns[index].gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the cooldown text of a certain skill.
        /// </summary>
        /// <param name="cooldown"> The cooldown of the skill. </param>
        /// <param name="index"> Which skill is meant. </param>
        public void CooldownSkill(float cooldown, int index)
        {
            if (cooldown <= 0)
            {
                skillIndicators[index].interactable = true;
                skillCooldowns[index].gameObject.SetActive(false);
                return;
            }

            skillCooldowns[index].text = cooldown.ToString("0.0");
        }
    }
}
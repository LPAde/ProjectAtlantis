using System.Collections.Generic;
using System.Globalization;
using Gameplay.Combat.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private List<Button> skillIndicators;
        [SerializeField] private List<TextMeshProUGUI> skillCooldowns;

        private void Start()
        {
            healthBar.maxValue = GameManager.Instance.Player.PlayerStats.MAXHealth;
            healthBar.value = GameManager.Instance.Player.PlayerStats.Health;
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
        public void UpdateSkills(CombatSpell[] combatSpells, MovementSpell movementSpell)
        {
            for (int i = 0; i < combatSpells.Length; i++)
            {
                skillIndicators[i].image.sprite = combatSpells[i].SpellSprite;
            }

            skillIndicators[3].image.sprite = movementSpell.SpellSprite;
        }

        public void UseSkill(int index)
        {
            skillIndicators[index].interactable = false;
            skillCooldowns[index].gameObject.SetActive(true);
        }

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
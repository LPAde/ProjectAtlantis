using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;

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
    }
}

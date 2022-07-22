using PlayerScripts;
using UnityEngine;

namespace Gameplay.Collectibles
{
    public class StatUpgradeItem : BaseItem
    {
        [SerializeField] private PlayerStats addedStats;
        
        /// <summary>
        /// Gives the player some stats.
        /// </summary>
        protected override void OnCollecting()
        {
            base.OnCollecting();
            GameManager.Instance.Player.UpgradeStats(addedStats);
        }
    }
}
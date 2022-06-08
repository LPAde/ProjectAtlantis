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
            GameManager.Instance.Player.UpgradeStats(addedStats);
            base.OnCollecting();
        }
    }
}
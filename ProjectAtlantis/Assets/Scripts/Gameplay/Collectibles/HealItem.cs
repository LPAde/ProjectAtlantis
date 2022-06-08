using UnityEngine;

namespace Gameplay.Collectibles
{
    public class HealItem : BaseItem
    {
        [SerializeField] private float value;

        protected override void OnCollecting()
        {
            GameManager.Instance.Player.Heal(value); 
            base.OnCollecting();
        }
    }
}

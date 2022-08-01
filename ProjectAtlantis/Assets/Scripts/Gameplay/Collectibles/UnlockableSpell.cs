using Gameplay.Combat.Spells;
using UnityEngine;

namespace Gameplay.Collectibles
{
    public class UnlockableSpell : BaseItem
    {
        [SerializeField] private BaseSpell spell;
        
        protected override void Update()
        {
        }
        
        protected override void OnCollecting()
        {
            base.OnCollecting();

            int currentlyCollectedSpells = SaveSystem.GetInt("UnlockedSpells");
            currentlyCollectedSpells++;
            SaveSystem.SetInt("UnlockedSpells", currentlyCollectedSpells);
            GameManager.Instance.SpellManager.UnlockSpell(spell);
        }
    }
}

using UnityEngine;

namespace Gameplay.Combat.Spells
{
    public abstract class BaseSpell : ScriptableObject
    {
        [SerializeField] protected Character owner;
        [SerializeField] protected float maxCoolDown;
        [SerializeField] protected float currentCoolDown;
        [SerializeField] protected float currentMaxCoolDown;
        
        [Header("Visual Stuff")]
        [SerializeField] private Sprite spellSprite;
        [SerializeField] private string description;
        
        public Sprite SpellSprite => spellSprite;
        public string Description => description;
        
        /// <summary>
        /// Sets the owner of the spell.
        /// </summary>
        /// <param name="newOwner"> The new Owner. </param>
        public void SetOwner(Character newOwner)
        {
            owner = newOwner;
            currentCoolDown = 0;
            currentMaxCoolDown = maxCoolDown;
        }

        public abstract bool Cast();

        public float TickDownCooldown()
        {
            currentCoolDown -= Time.deltaTime;
            
            return currentCoolDown;
        }

        public void ReduceMaxCooldown(float cdr)
        {
            currentMaxCoolDown = maxCoolDown - (maxCoolDown * cdr);
        }
    }
}
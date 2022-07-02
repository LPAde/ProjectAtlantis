using UnityEngine;

namespace Gameplay.Combat.Spells
{
    public abstract class BaseSpell : ScriptableObject
    {
        [SerializeField] protected Character owner;
        [SerializeField] protected float maxCoolDown;
        [SerializeField] protected float currentCoolDown;
        
        /// <summary>
        /// Sets the owner of the spell.
        /// </summary>
        /// <param name="newOwner"> The new Owner. </param>
        public void SetOwner(Character newOwner)
        {
            owner = newOwner;
            currentCoolDown = 0;
        }
        
        public virtual void Cast()
        {
        }

        public void TickDownCooldown()
        {
            currentCoolDown -= Time.deltaTime;
        }
    }
}
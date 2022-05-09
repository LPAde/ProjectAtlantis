using UnityEngine;

namespace Combat.Spells
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
            currentCoolDown = maxCoolDown;
        }
        
        public virtual void Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                return;
            }

            currentCoolDown = maxCoolDown;
        }

        public void TickDownCooldown()
        {
            currentCoolDown -= Time.deltaTime;
        }
    }
}
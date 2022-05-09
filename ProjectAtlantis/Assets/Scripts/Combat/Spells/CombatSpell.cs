using UnityEngine;

namespace Combat.Spells
{
    public abstract class CombatSpell : BaseSpell
    {
        [SerializeField] protected float damageValue;
    }
}

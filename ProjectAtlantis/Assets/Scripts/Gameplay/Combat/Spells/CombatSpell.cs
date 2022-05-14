using UnityEngine;

namespace Gameplay.Combat.Spells
{
    public abstract class CombatSpell : BaseSpell
    {
        [SerializeField] protected float damageValue;
    }
}

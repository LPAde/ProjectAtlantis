using UnityEngine;

namespace Gameplay.Combat.Spells
{
    public abstract class MovementSpell : BaseSpell
    {
        [SerializeField] protected float travelDistance;
    }
}
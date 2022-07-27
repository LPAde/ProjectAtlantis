using Enemies;
using PlayerScripts;
using UnityEngine;

namespace Gameplay.Combat.Spells
{
    [CreateAssetMenu(fileName = "Dash Spell", menuName = "Spells/Movement Spell/Dash Spell", order = 0)]
    public class DashSpell : MovementSpell
    {
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashSpeed;
        
        public override bool Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                return false;
            }
            
            currentCoolDown = maxCoolDown;
            
            // Player Spell.
            Player owningPlayer = (Player) owner;

            // Calculates own Vector.
            var travelVector = travelDistance *
                               owningPlayer.ProjectileSpawnPosition.forward;
            
            // Tells player to dash.
            owningPlayer.PlayerController.InitializeDash(travelVector, dashDuration, dashSpeed);

            return true;
        }
    }
}
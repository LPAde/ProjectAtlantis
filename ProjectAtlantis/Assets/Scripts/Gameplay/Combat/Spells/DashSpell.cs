using Enemies;
using PlayerScripts;
using UnityEngine;

namespace Gameplay.Combat.Spells
{
    [CreateAssetMenu(fileName = "Dash Spell", menuName = "Spells/Movement Spell/Dash Spell", order = 0)]
    public class DashSpell : MovementSpell
    {
        public override void Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                Debug.Log("retunr");
                return;
            }
            
            currentCoolDown = maxCoolDown;
            
            // Check who is owner.
            if (owner is BaseEnemy owningEnemy)
            {
                // Enemy Spell.
                
            }
            else
            {
                // Player Spell.
                Player owningPlayer = (Player) owner;

                var travelVector = travelDistance *
                                   owningPlayer.ProjectileSpawnPosition.forward;
                
                // Uses the character controller to make the player dash forward.
                owningPlayer.CharacterController.Move(travelVector);
                owningPlayer.PlayerController.UpdateMovePos(travelVector);
            }
        }
    }
}
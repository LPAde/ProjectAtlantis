using System;
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
            
            currentCoolDown = currentMaxCoolDown;
            
            // Player Spell.
            Player owningPlayer = (Player) owner;

            // Calculates own Vector.
            var travelVector = travelDistance *
                               owningPlayer.ProjectileSpawnPosition.forward;

            switch (GameManager.Instance.RhythmManager.CheckTiming())
            {
                case Timing.Bad:
                    travelVector *= .2f;
                    break;
                case Timing.Good:
                    travelVector *= .8f;
                    break;
                case Timing.Amazing:
                    travelVector *= 1.2f;
                    break;
                case Timing.Perfect:
                    travelVector *= 2f;
                    break;
            }
            
            // Tells player to dash.
            owningPlayer.PlayerController.InitializeDash(travelVector, dashDuration, dashSpeed);

            return true;
        }
    }
}
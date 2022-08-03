using System.Collections.Generic;
using Gameplay.Combat.Projectiles.PlayerProjectiles;
using PlayerScripts;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Combat.Spells
{
    [CreateAssetMenu(fileName = "Bubble Bash", menuName = "Spells/Movement Spell/Bubble Bash", order = 1)]
    public class BubbleBash : DashSpell
    {
        [SerializeField] private List<GameObject> projectiles;

        public override bool Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                return false;
            }
            
            base.Cast();

            Player owningPlayer = (Player)owner;

            for (int i = 0; i < projectiles.Count; i++)
            {
                Debug.Log("here");
                var proj = Instantiate(projectiles[i], owningPlayer.BubblePositions[i].position, quaternion.identity, owningPlayer.transform).GetComponent<PlayerProjectile>();
                proj.Initialize(owningPlayer.BubblePositions[i].position - owningPlayer.PlayerController.transform.position, GameManager.Instance.RhythmManager.CheckTiming());
            }
            
            currentCoolDown = maxCoolDown;

            return true;
        }
    }
}

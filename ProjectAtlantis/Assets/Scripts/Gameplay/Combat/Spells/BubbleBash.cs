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
        [SerializeField] private List<Vector3> projectileVectors;

        public override void Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                return;
            }
            
            currentCoolDown = maxCoolDown;
            
            base.Cast();

            Player owningPlayer = (Player)owner;

            for (int i = 0; i < projectiles.Count; i++)
            {
                var proj = Instantiate(projectiles[i], owningPlayer.ProjectileSpawnPosition.position, quaternion.identity, owningPlayer.transform).GetComponent<PlayerProjectile>();
                proj.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), projectileVectors[i]);
            }
        }
    }
}

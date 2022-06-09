using Enemies;
using Gameplay.Combat.Projectiles;
using Gameplay.Combat.Projectiles.EnemyProjectiles;
using Gameplay.Combat.Projectiles.PlayerProjectiles;
using PlayerScripts;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Combat.Spells
{
    [CreateAssetMenu(fileName = "Projectile Spell",menuName = "Spells/Combat Spell/Projectile Spawning Spell", order = 0)]
    public class ProjectileSpawningSpell : CombatSpell
    {
        [SerializeField] private GameObject projectile;

        public override void Cast()
        {
            // Stops when the spell is still on cooldown.
            if (currentCoolDown > 0)
            {
                Debug.Log("ON CD");
                return;
            }
            
            currentCoolDown = maxCoolDown;

            // Check who is owner.
            if (owner is AttackingEnemy owningEnemy)
            {
                // Enemy Spell.
                var position = owningEnemy.ProjectileSpawnPosition.position;
                var proj =
                Instantiate(projectile, position, Quaternion.identity,
                    GameManager.Instance.transform).GetComponent<EnemyProjectile>();
                proj.Initialize(GameManager.Instance.Player.PlayerController.transform.position - position);
            }
            else
            {
                // Player Spell.
                var owningPlayer = (Player) owner;
                
                var proj = Instantiate(projectile, owningPlayer.ProjectileSpawnPosition.position, quaternion.identity, owningPlayer.transform).GetComponent<PlayerProjectile>();
                proj.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), owningPlayer.ProjectileSpawnPosition.forward);
            }
        }
    }
}

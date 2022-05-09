using Enemies;
using PlayerScripts;
using Unity.Mathematics;
using UnityEngine;

namespace Combat.Spells
{
    [CreateAssetMenu(fileName = "Projectile Spell",menuName = "Spells/Combat Spell/Projectile Spawning Spell", order = 0)]
    public class ProjectileSpawningSpell : CombatSpell
    {
        [SerializeField] private GameObject projectile;

        public override void Cast()
        {
            base.Cast();

            // Check who is owner.
            if (owner is BaseEnemy owningEnemy)
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
                Player owningPlayer = (Player) owner;
                
                var proj = Instantiate(projectile, owningPlayer.ProjectileSpawnPosition.position, quaternion.identity, owningPlayer.transform).GetComponent<PlayerProjectile>();
                                    proj.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), owningPlayer.ProjectileSpawnPosition.forward);
            }
        }
    }
}

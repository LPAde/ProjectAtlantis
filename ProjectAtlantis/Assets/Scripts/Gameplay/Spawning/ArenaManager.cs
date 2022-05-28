using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Spawning
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private List<BaseEnemy> arenaEnemies;
        [SerializeField] private List<float> heightChecks;
        [SerializeField] private List<float> difficultyModifiers;

        [SerializeField] private bool isInArena;

        private bool spawningFinished;

        public bool IsInArena => isInArena;
        
        private void Awake()
        {
            GameManager.Instance.EnemySpawner.OnFinishedSpawning += FinishSpawning;
        }
        
        private void FinishSpawning()
        {
            spawningFinished = true;
        }

        /// <summary>
        /// Identifies the correct difficulty modifier based on the player height.
        /// </summary>
        /// <returns> The fitting difficulty modifier. </returns>
        public float GetDifficultyModifier()
        {
            float playerHeight = GameManager.Instance.Player.PlayerController.transform.position.y;

            if (playerHeight > heightChecks[0])
                return difficultyModifiers[0];
            if(playerHeight > heightChecks[1])
                return difficultyModifiers[1];
            
            return difficultyModifiers[2];
        }
        
        public void AddArenaEnemy(BaseEnemy newArenaEnemy)
        {
            arenaEnemies.Add(newArenaEnemy);
        }

        public void RemoveArenaEnemy(BaseEnemy formerArenaEnemy)
        {
            arenaEnemies.Remove(formerArenaEnemy);

            if (arenaEnemies.Count > 0) 
                return;
            
            if(!spawningFinished)
                return;

            GameManager.Instance.EnemySpawner.StartSpawning();
        }
    }
}
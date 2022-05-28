using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Gameplay.Spawning
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private int currentWave;
        [SerializeField] private List<int> hardcodedWaves;
        [SerializeField] private List<Wave> hardcodedEnemies;

        public List<BaseEnemy> GenerateNextWave(List<BaseEnemy> lastWave)
        {
            currentWave++;
            
            // Checks if we are in a hardcoded Wave.
            for (int i = 0; i < hardcodedWaves.Count; i++)
            {
                if (currentWave == hardcodedWaves[i])
                {
                    return hardcodedEnemies[i].CreateEnemyList();
                }
                else
                {
                    if (hardcodedWaves[i] > currentWave)
                        break;
                }
            }
            
            // Setting up the spawning.
            float formerCombatScore = lastWave.Sum(enemy => enemy.CombatScore);
            formerCombatScore *= GameManager.Instance.ArenaManager.GetDifficultyModifier();
            float newCombatScore = 0;
            List<BaseEnemy> newWave = new List<BaseEnemy>();
            
            // Adding enemies to the new list till the new combat score is bigger, then the former combat score.
            while (newCombatScore < formerCombatScore)
            {
                formerCombatScore++;
            }

            return newWave;
        }
    }

    [Serializable]
    public struct Wave
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private List<int> enemyAmount;

        /// <summary>
        /// Creates a list of base enemies based on the input of the wave.
        /// </summary>
        /// <returns> The newly created list. </returns>
        public List<BaseEnemy> CreateEnemyList()
        {
            List<BaseEnemy> newBaseEnemies = new List<BaseEnemy>();

            // Do something for every enemy.
            for (int i = 0; i < enemies.Count; i++)
            {
                // Do it the amount of predetermined types.
                for (int j = 0; j < enemyAmount[i]; j++)
                {
                    newBaseEnemies.Add(enemies[i]);
                }
            }

            return newBaseEnemies;
        }
    }
}
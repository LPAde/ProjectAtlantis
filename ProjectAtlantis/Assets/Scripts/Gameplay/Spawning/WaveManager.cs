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
                    return hardcodedEnemies[i].Enemies;
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

        public List<BaseEnemy> Enemies => enemies;
    }
}
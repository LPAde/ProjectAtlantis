using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Spawning
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private int currentWave;
        
        [Header("Hardcoded Enemy Waves")]
        [SerializeField] private List<int> hardcodedWavesIndicator;
        [SerializeField] private List<Wave> hardcodedWaves;

        [Header("Item Related Stuff")] 
        [SerializeField] private List<GameObject> keys;
        [SerializeField] private List<int> keySpawnWaves;
        [SerializeField] private List<GameObject> walls;
        
        
        public int CurrentWave => currentWave;

        private void Awake()
        {
            GameManager.Instance.Load += Load;
        }

        public List<BaseEnemy> GenerateNextWave(List<BaseEnemy> lastWave)
        {
            currentWave++;
            
            WaveItemCheck();
            
            // Checks if we are in a hardcoded Wave.
            for (int i = 0; i < hardcodedWavesIndicator.Count; i++)
            {
                if (currentWave == hardcodedWavesIndicator[i])
                {
                    return hardcodedWaves[i].CreateEnemyList();
                }
                else
                {
                    if (hardcodedWavesIndicator[i] > currentWave)
                        break;
                }
            }
            
            // Setting up the spawning.
            float formerCombatScore = lastWave.Sum(enemy => enemy.CombatScore);
            formerCombatScore *= GameManager.Instance.ArenaManager.GetDifficultyModifier();
            
            float newCombatScore = 0;
            List<BaseEnemy> newWave = new List<BaseEnemy>();
            List<BaseEnemy> possibleEnemies = GameManager.Instance.ArenaManager.GetPossibleEnemies(currentWave);

            // Adding enemies to the new list till the new combat score is bigger, then the former combat score.
            while (newCombatScore < formerCombatScore)
            {
                // Filling the upper 25% with popcorn.
                if (formerCombatScore * .75f <= newCombatScore)
                {
                    newWave.Add(possibleEnemies[0]);
                    newCombatScore += possibleEnemies[0].CombatScore;
                }
                else
                {
                    // Adding random enemies from the generated 100 enemy list.
                    int random = Random.Range(0, 101);
                    newWave.Add(possibleEnemies[random]);
                    newCombatScore += possibleEnemies[random].CombatScore;
                }
            }
            
            return newWave;
        }

        public void DestroyWall(int collectedKeys)
        {
            Destroy(walls[collectedKeys-1]);
        }

        private void WaveItemCheck()
        {
            if (keySpawnWaves.Contains(currentWave))
            {
                int index = 0;
                for (int i = 0; i < keySpawnWaves.Count; i++)
                {
                    if (keySpawnWaves[i] == currentWave)
                    {
                        index = i;
                        break;
                    }
                }
                
                if(walls[index] == null)
                    return;

                keys[index].SetActive(true);
            }
        }
        
        private void Load()
        {
            int keyAmount = SaveSystem.GetInt("UsedKeys");
            
            if (keyAmount == 0)
                return;
            
            // Remove and destroy all unlocked walls.
            for (int i = 0; i < keyAmount; i++)
            {
                if(i >= walls.Count)
                    break;
                
                Destroy(walls[i]);
            }
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
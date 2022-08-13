using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Spawning
{
    public class ArenaManager : MonoBehaviour
    {
        [Header("Player related Stuff")]
        [SerializeField] private List<float> heightChecks;
        [SerializeField] private List<float> difficultyModifiers;
        [SerializeField] private bool isInArena;
        
        [Header("Enemy related Stuff")]
        [SerializeField] private List<BaseEnemy> allEnemies;
        [SerializeField] private List<BaseEnemy> arenaEnemies;
        [SerializeField] private List<int> spawnRates;
        [SerializeField] private int spawnChangeIndicator;

        [Header("Misc")] 
        [SerializeField] private float currentDuration;
        [SerializeField] private float baseDuration;
        [SerializeField] private float durationIncrease;

        public List<float> HeightChecks => heightChecks;
        public bool IsInArena => isInArena;

        #region Unity Methods

        private void Awake()
        {
            GameManager.Instance.EnemySpawner.OnWaveStart += FixDuration;
            GameManager.Instance.EnemySpawner.OnWaveStart += FixSpawnRates;
        }

        private void Update()
        {
            currentDuration -= Time.deltaTime;

            // Starts next wave when either the time is up or no enemies are cumming up.
            if (currentDuration <= 0 || !GameManager.Instance.EnemySpawner.IsSpawning && arenaEnemies.Count == 0)
            {
                // Don't start when there are max enemies on the field.
                if(arenaEnemies.Count >= GameManager.Instance.EnemySpawner.MaxEnemyAmount)
                {
                    currentDuration += durationIncrease;
                    return;
                }
                
                GameManager.Instance.EnemySpawner.OnWaveStart.Invoke(GameManager.Instance.WaveManager.CurrentWave);
            }
        }

        #endregion
        

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

        /// <summary>
        /// Creates a list with all the possible enemies.
        /// </summary>
        /// <param name="currentWave"> The wave to determine the list. </param>
        /// <returns> A list with 100 enemies that a wave can be generated from. </returns>
        public List<BaseEnemy> GetPossibleEnemies(int currentWave)
        {
            // Set-up
            List<BaseEnemy> possibleEnemies = new List<BaseEnemy>();
            float playerHeight = GameManager.Instance.Player.PlayerController.transform.position.y;
            possibleEnemies.Add(allEnemies[0]);
            
            /*
             * 0 = Popcorn
             * 1 = Jellycanon
             * 2 = Eel
             * 3 = Piranhadude
             * 4 = Stronk Piranhadude
             * 5 = Shotguncrab
             * 6 = Jellycanon 2
             */
            if (playerHeight > heightChecks[0])
            {
                for (int i = 0; i < spawnRates.Count; i++)
                {
                    // Preventing mistakes.
                    if(spawnRates[i] <= 0)
                        continue;
                    
                    for (int j = 0; j < spawnRates[i]; j++)
                    {
                        switch (i)
                        {
                           case 0:
                               possibleEnemies.Add(allEnemies[0]);
                               break;
                           case 1:
                               possibleEnemies.Add(allEnemies[1]);
                               break;
                            case 2:
                                possibleEnemies.Add(allEnemies[2]);
                                break;
                            case 3:
                                possibleEnemies.Add(allEnemies[3]);
                                break;
                        }
                    }
                }
            }
            else if (playerHeight > heightChecks[1])
            {
                for (int i = 0; i < spawnRates.Count; i++)
                {
                    // Preventing mistakes.
                    if(spawnRates[i] <= 0)
                        continue;

                    for (int j = 0; j < spawnRates[i]; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                possibleEnemies.Add(allEnemies[0]);
                                break;
                            case 1:
                                possibleEnemies.Add(allEnemies[6]);
                                break;
                            case 2:
                                possibleEnemies.Add(allEnemies[3]);
                                break;
                            case 3:
                                possibleEnemies.Add(allEnemies[4]);
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < spawnRates.Count; i++)
                {
                    // Preventing mistakes.
                    if(spawnRates[i] <= 0)
                        continue;

                    for (int j = 0; j < spawnRates[i]; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                possibleEnemies.Add(allEnemies[0]);
                                break;
                            case 1:
                                possibleEnemies.Add(allEnemies[6]);
                                break;
                            case 2:
                                possibleEnemies.Add(allEnemies[4]);
                                break;
                            case 3:
                                possibleEnemies.Add(allEnemies[5]);
                                break;
                        }
                    }
                }
            }
            
            return possibleEnemies;
        }
        
        public void AddArenaEnemy(BaseEnemy newArenaEnemy)
        {
            arenaEnemies.Add(newArenaEnemy);
        }

        public void RemoveArenaEnemy(BaseEnemy formerArenaEnemy)
        {
            arenaEnemies.Remove(formerArenaEnemy);
        }

        private void FixDuration(int currentWave)
        {
            currentDuration = baseDuration + durationIncrease * currentWave;
        }

        private void FixSpawnRates(int currentWave)
        {
            // Only fixes them every tenth wave.
            if (currentWave % spawnChangeIndicator != 0)
                return;
            
            spawnRates[0] -= 10;
            spawnRates[1] += 5;
            spawnRates[2] += 10;
            spawnRates[3] += 5;

            int combinedPositiveSpawnRates = 0;

            for (int i = 0; i < spawnRates.Count; i++)
            {
                if (spawnRates[i] > 0)
                    combinedPositiveSpawnRates += spawnRates[i];
            }
            
            // Fixing wrong spawn-rates by adding or removing popcorn.
            if (combinedPositiveSpawnRates != 100)
            {
                spawnRates[0] += 100 - combinedPositiveSpawnRates;
            }

            // Removing the method after it reached it's low. 
            if (spawnRates[0] == 10)
                GameManager.Instance.EnemySpawner.OnWaveStart -= FixSpawnRates;
        }
    }
}
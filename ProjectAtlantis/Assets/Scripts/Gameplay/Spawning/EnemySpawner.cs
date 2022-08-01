using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Spawning
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<BaseEnemy> enemies;
        [SerializeField] private List<Transform> spawnPositionsHeightOne;
        [SerializeField] private List<Transform> spawnPositionsHeightTwo;
        [SerializeField] private List<Transform> spawnPositionsHeightThree;
        [SerializeField] private float enemySpawnDelay;
        [SerializeField] private int maxEnemyAmount;
        private int _currentSpawnPosition;

        public Action<int> OnWaveStart;
        
        public bool IsSpawning { get; private set; }

        private void Awake()
        {
            OnWaveStart += UpdateWave;
            OnWaveStart += StartSpawning;
        }

        /// <summary>
        /// Starts spawning the currently saved enemies.
        /// </summary>
        private void StartSpawning(int wave)
        {
            IsSpawning = true;
            StartCoroutine(SpawnEnemies());
        }

        /// <summary>
        /// Determines the point at which an enemy shall be spawned.
        /// </summary>
        /// <returns> The point something shall be spawned at. </returns>
        private Vector3 GetSpawnPoint()
        {
            _currentSpawnPosition++;
            float playerHeight = GameManager.Instance.Player.PlayerController.transform.position.y;
            var heightChecks = GameManager.Instance.ArenaManager.HeightChecks;

            // Case: Player is at the top.
            if (playerHeight > heightChecks[0])
            {
                
                if (_currentSpawnPosition >= spawnPositionsHeightOne.Count)
                    _currentSpawnPosition = 0;

                return spawnPositionsHeightOne[_currentSpawnPosition].position;
            }

            // Case: Player is in the middle.
            if(playerHeight > heightChecks[1])
            {
                
                if (_currentSpawnPosition >= spawnPositionsHeightTwo.Count)
                    _currentSpawnPosition = 0;

                return spawnPositionsHeightTwo[_currentSpawnPosition].position;
            }

            // Case: Player is at the bottom.
            if (_currentSpawnPosition >= spawnPositionsHeightThree.Count)
                _currentSpawnPosition = 0;

            return spawnPositionsHeightThree[_currentSpawnPosition].position;
        }

        private void UpdateWave(int wave)
        {
            enemies = GameManager.Instance.WaveManager.GenerateNextWave(enemies);
        }

        /// <summary>
        /// Creates a spawn delay for the spawns depending on the possible spawn points at the current height.
        /// </summary>
        /// <param name="height"> The current height of the spawn point. </param>
        /// <returns> The delay for the next spawn. </returns>
        private float GetSpawnDelay(float height)
        {
            float tempSpawnDelay = enemySpawnDelay;
            
            if (Math.Abs(height - spawnPositionsHeightOne[0].position.y) < .0001f)
            {
                return tempSpawnDelay / spawnPositionsHeightOne.Count;
            }

            if (Math.Abs(height - spawnPositionsHeightTwo[0].position.y) < .0001f)
            {
                return tempSpawnDelay / spawnPositionsHeightTwo.Count;
            }

            return tempSpawnDelay / spawnPositionsHeightThree.Count;
        }

        /// <summary>
        /// Spawns a list of enemies with a small delay between each of them.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnEnemies()
        {
            foreach (var enemy in enemies)
            {
                while (GameManager.Instance.EnemyManager.Enemies.Count >= maxEnemyAmount)
                    yield return new WaitForSeconds(enemySpawnDelay * 3);
                
                var spawnPoint = GetSpawnPoint();
                var en = Instantiate(enemy,spawnPoint,quaternion.identity,transform);
                
                GameManager.Instance.ArenaManager.AddArenaEnemy(en);
                en.MakeArenaEnemy();

                yield return new WaitForSeconds(GetSpawnDelay(spawnPoint.y));
            }

            IsSpawning = false;
        }
    }
}
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
        [SerializeField] private List<Transform> spawnPositions;
        [SerializeField] private float enemySpawnDelay;
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
        /// Updates the current spawn point.
        /// </summary>
        private void UpdateSpawnPoints()
        {
            if (spawnPositions.Count < 2)
                return;

            _currentSpawnPosition++;

            if (_currentSpawnPosition >= spawnPositions.Count)
                _currentSpawnPosition = 0;
        }

        private void UpdateWave(int wave)
        {
            enemies = GameManager.Instance.WaveManager.GenerateNextWave(enemies);
        }

        /// <summary>
        /// Spawns a list of enemies with a small delay between each of them.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnEnemies()
        {
            foreach (var enemy in enemies)
            {
                UpdateSpawnPoints();
                var en = Instantiate(enemy,spawnPositions[_currentSpawnPosition].position,quaternion.identity,transform);
                
                GameManager.Instance.ArenaManager.AddArenaEnemy(en);
                
                yield return new WaitForSeconds(enemySpawnDelay);
            }

            IsSpawning = false;
        }
    }
}
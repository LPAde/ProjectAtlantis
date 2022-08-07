using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<BaseEnemy> enemies;

        public List<BaseEnemy> Enemies => enemies;

        private void Update()
        {
            foreach (var enemy in enemies)
            {
                 enemy.EnemyUpdate();
            }
        }

        public void AddEnemy(BaseEnemy newEnemy)
        {
            enemies.Add(newEnemy);
        }

        public void RemoveEnemy(BaseEnemy formerEnemy)
        {
            enemies.Remove(formerEnemy);
        }

        /// <summary>
        /// Gets the closest enemy to a certain position.
        /// </summary>
        /// <param name="currentEnemy"> The position you want the closest enemy to. </param>
        /// <returns> The closest enemy. </returns>
        public BaseEnemy GetClosestEnemy(BaseEnemy currentEnemy)
        {
            // Stops when there are no enemies alive.
            if (enemies[0] == null || enemies[1] == null)
                return null; 
            
            // Set up.
            BaseEnemy closestEnemy = enemies[0];

            if (currentEnemy == enemies[0] && enemies[1] != null)
                closestEnemy = enemies[1];
                
            
            float currentClosestSqrDistance = (closestEnemy.transform.position - currentEnemy.transform.position).sqrMagnitude;

            // Finds closest enemy.
            for (int i = 1; i < enemies.Count; i++)
            {
                // Skip if exact same position.
                if(currentEnemy == enemies[i])
                    continue;
                
                float currentDistance = (enemies[i].transform.position - currentEnemy.transform.position).sqrMagnitude;
                
                if (currentDistance < currentClosestSqrDistance)
                {
                    closestEnemy = enemies[i];
                    currentClosestSqrDistance = currentDistance;
                }
            }

            return closestEnemy;
        }
    }
}

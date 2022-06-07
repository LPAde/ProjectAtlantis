using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<BaseEnemy> enemies;

        public List<BaseEnemy> Enemies => enemies;

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
        /// <param name="currentPosition"> The position you want the closest enemy to. </param>
        /// <returns> The closest enemy. </returns>
        public BaseEnemy GetClosestEnemy(Vector3 currentPosition)
        {
            // Stops when there are no enemies alive.
            if (enemies[0] == null)
                return null; 
            
            // Set up.
            BaseEnemy closestEnemy = enemies[0];
            float currentClosestSqrDistance = (closestEnemy.transform.position - currentPosition).sqrMagnitude;

            // Finds closest enemy.
            for (int i = 1; i < enemies.Count; i++)
            {
                // Skip if exact same position.
                if(currentPosition == enemies[i].transform.position)
                    continue;
                
                float currentDistance = (enemies[i].transform.position - currentPosition).sqrMagnitude;
                
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

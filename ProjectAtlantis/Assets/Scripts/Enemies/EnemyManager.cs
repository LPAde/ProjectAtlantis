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
    }
}

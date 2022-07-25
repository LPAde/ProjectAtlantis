using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Combat.Projectiles.PlayerProjectiles
{
    public class Harpune : PlayerProjectile
    {
        [SerializeField] private List<BaseEnemy> hitEnemies;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Enemy"))
                return;

            var en = other.GetComponent<BaseEnemy>();
            
            if(hitEnemies.Contains(en))
                return;
            
            hitEnemies.Add(en);
            
            en.TakeDamage(damage, (en.transform.position - GameManager.Instance.Player.PlayerController.transform.position).normalized * 200, 1);
        }
    }
}
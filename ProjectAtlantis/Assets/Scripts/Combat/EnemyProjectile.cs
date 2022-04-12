using UnityEngine;

namespace Combat
{
    public class EnemyProjectile : Projectile
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.Player.TakeDamage(damage);
            }
        
            base.OnTriggerEnter(other);
        }
    }
}

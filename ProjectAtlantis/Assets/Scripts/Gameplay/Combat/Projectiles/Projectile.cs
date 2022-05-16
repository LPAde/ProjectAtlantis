using UnityEngine;

namespace Gameplay.Combat.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected Vector3 movementVector;

        private void FixedUpdate()
        {
            transform.position += movementVector;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ground") || other.CompareTag("Projectile"))
                return;
        
            Destroy(gameObject);
        }

        public void Initialize(Vector3 newMovementVector)
        {
            movementVector = newMovementVector * projectileSpeed;
        }
    }
}
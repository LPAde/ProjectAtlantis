using UnityEngine;

namespace Combat
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
            Destroy(gameObject);
        }

        public void Initialize(Vector3 newMovementVector)
        {
            movementVector = newMovementVector * projectileSpeed;
        }
    }
}

using System;
using UnityEngine;

namespace Gameplay.Combat.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected Vector3 movementVector;

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            
            if(lifeTime < 0)
                Destroy(gameObject);
        }

        protected virtual void FixedUpdate()
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

        public virtual void Initialize(Vector3 newMovementVector, Timing timing)
        {
            Initialize(newMovementVector);
        }
    }
}
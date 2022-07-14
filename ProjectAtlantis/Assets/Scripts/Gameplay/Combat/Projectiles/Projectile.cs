using UnityEngine;

namespace Gameplay.Combat.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected Vector3 movementVector;
        [SerializeField] private float maxDistanceToGround = 1.5f;
        
        protected virtual void Update()
        {
            lifeTime -= Time.deltaTime;
            
            if(lifeTime < 0)
                Destroy(gameObject);
        }

        protected virtual void FixedUpdate()
        {
            var hit = new RaycastHit();
            var tempMovementVector = movementVector;
            
            if (Physics.Raycast (transform.position, -Vector3.up, out hit))
            {
                if(!hit.transform.CompareTag("Ground"))
                    return;
                
                tempMovementVector.y -= (hit.distance - maxDistanceToGround);
            }
            
            transform.position += tempMovementVector;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Projectile") || other.CompareTag("Area Hitbox"))
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
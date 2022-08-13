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
        [SerializeField] private AudioSource source;
        
        [Header("Death Particle related stuff")]
        [SerializeField] protected GameObject deathParticleObject;
        [SerializeField] protected float deathParticleUpTime;

        private void Awake()
        {
            if(source != null)
                source.Play();
        }

        protected virtual void Update()
        {
            lifeTime -= Time.deltaTime;
            
            if(lifeTime < 0)
                Destroy(gameObject);
        }

        protected virtual void FixedUpdate()
        {
            var tempMovementVector = movementVector;
            
            // Fix height a bit to ensure the distance to the ground is being held.
            if (Physics.Raycast (transform.position, -Vector3.up, out var hit))
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

        protected virtual void OnDestroy()
        {
            if(lifeTime <= 0)
                return;
            
            if (deathParticleObject != null)
            {
                var particle = Instantiate(deathParticleObject, transform.position, Quaternion.identity, GameManager.Instance.transform);
                Destroy(particle, deathParticleUpTime);
            }
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
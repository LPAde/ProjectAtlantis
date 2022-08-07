using System.Security.Cryptography;
using UnityEngine;

namespace Gameplay.Collectibles
{
    public abstract class BaseItem : MonoBehaviour
    {
        [SerializeField] private float upTime;

        [Header("Particle Related Stuff")] 
        [SerializeField] private GameObject ParticleObject;
        [SerializeField] private float particleUpTime;

        protected virtual void Update()
        {
            upTime -= Time.deltaTime;
            
            if(upTime < 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            
            var particle = Instantiate(ParticleObject, transform.position, Quaternion.identity, GameManager.Instance.transform);
            Destroy(particle, particleUpTime);
            
            OnCollecting();
        }

        /// <summary>
        /// What happens when the Item is collected.
        /// </summary>
        protected virtual void OnCollecting()
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace Gameplay.Collectibles
{
    public abstract class BaseItem : MonoBehaviour
    {
        [SerializeField] private float upTime;

        private void Update()
        {
            upTime -= Time.deltaTime;
            
            if(upTime < 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
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

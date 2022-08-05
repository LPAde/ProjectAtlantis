using UnityEngine;

namespace PlayerScripts
{
    public class MovementIndicator : MonoBehaviour
    {
        [SerializeField] private float maxUpTime;
        [SerializeField] private float upTime;
        
        private void Update()
        {
            upTime -= Time.deltaTime;
            
            if(upTime < 0)
                gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the indicator at desired position.
        /// </summary>
        /// <param name="newPosition"> The position it should be at. </param>
        public void Activate(Vector3 newPosition)
        {
            gameObject.SetActive(true);
            upTime = maxUpTime;
            transform.position = newPosition;
        }
    }
}
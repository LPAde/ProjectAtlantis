using UnityEngine;

namespace UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float destroyTime;
        [SerializeField] private Vector3 offset;
        
        void Start()
        {
            Destroy(gameObject,destroyTime);

            transform.localPosition += offset;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation( transform.position - Camera.main.transform.position );
        }
    }
}

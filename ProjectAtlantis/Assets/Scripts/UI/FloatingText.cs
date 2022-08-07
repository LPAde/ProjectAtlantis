using Enemies;
using UnityEngine;

namespace UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float destroyTime;
        [SerializeField] private Vector3 offset;
        public TextMesh text;
        public BaseEnemy owner;
        
        void Start()
        {
            Destroy(gameObject,destroyTime);
        }

        private void Update()
        {
            if (owner == null)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.localPosition = owner.transform.position + offset;
            }
        }
    }
}

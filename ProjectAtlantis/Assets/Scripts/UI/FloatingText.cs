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
            transform.localPosition = owner.transform.position + offset;
        }
    }
}

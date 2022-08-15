using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private Transform finalHeight;

        private void Start()
        {
            // Prevent the resolution hurting the credits.
            scrollSpeed = (finalHeight.position.y - transform.position.y) * .02f;
        }

        void Update()
        {
            transform.position += new Vector3(0,Time.deltaTime * scrollSpeed,0);

            if (!(transform.position.y > finalHeight.position.y) && !Input.GetKeyDown(KeyCode.Escape)) 
                return;
            
            SceneManager.LoadScene(0);
        }
    }
}

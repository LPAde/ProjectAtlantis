using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private float finalHeight;

        private void Start()
        {
            // Prevent the resolution hurting the credits.
            Debug.Log(finalHeight);
            finalHeight *= Screen.currentResolution.width / 1280;
            Debug.Log(finalHeight);
            scrollSpeed *= Screen.currentResolution.width / 1280;
        }

        void Update()
        {
            transform.position += new Vector3(0,Time.deltaTime * scrollSpeed,0);

            if (!(transform.position.y > finalHeight) && !Input.GetKeyDown(KeyCode.Escape)) 
                return;
            
            SceneManager.LoadScene(0);
        }
    }
}

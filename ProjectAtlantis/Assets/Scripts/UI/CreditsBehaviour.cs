using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private float finalHeight;
        
        void Update()
        {
            transform.position += new Vector3(0,Time.deltaTime * scrollSpeed,0);
            
            if (transform.position.y > finalHeight || Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
        }
    }
}

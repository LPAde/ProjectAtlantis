using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private float duration;
        
        void Update()
        {
            transform.position += new Vector3(0,Time.deltaTime * scrollSpeed,0);

            duration -= Time.deltaTime;

            if (duration < 0 || Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
        }
    }
}

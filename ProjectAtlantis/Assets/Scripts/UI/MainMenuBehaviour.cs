using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public void OnStartGameClick()
        {
            SceneManager.LoadScene(1);
        }
        
        public void OnDeleteClick()
        {
            
        }
        
        public void OnCreditsClick()
        {
            SceneManager.LoadScene(2);
        }
    }
}
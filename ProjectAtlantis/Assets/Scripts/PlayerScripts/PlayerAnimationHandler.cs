using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerScripts
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        public void LookAtMovement()
        {
            GameManager.Instance.Player.PlayerController.LookAtMovement();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

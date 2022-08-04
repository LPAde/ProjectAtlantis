using UnityEngine;

namespace PlayerScripts
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        public void LookAtMovement()
        {
            GameManager.Instance.Player.PlayerController.LookAtMovement();
        }
    }
}

using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        [SerializeField] private GameObject playerAttack;

        public GameObject PlayerAttack => playerAttack;
    }
}

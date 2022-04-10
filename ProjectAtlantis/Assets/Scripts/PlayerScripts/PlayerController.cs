using Unity.Mathematics;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;
        
        [Header("Movement")]
        [SerializeField] private Vector3 movementVector;
        [SerializeField] private float speed;

        private void Start()
        {
            movementVector = transform.position;
        }

        private void Update()
        {
            CheckInputs();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var projectile = Instantiate(player.PlayerAttack, player.ProjectileSpawnPosition.position, quaternion.identity, player.transform).GetComponent<PlayerProjectile>();
                projectile.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), Vector3.forward);
            }

            movementVector += new Vector3(Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));
        }

        /// <summary>
        /// Moves the player to the established position. (Will be done with the animator later.) 
        /// </summary>
        private void Move()
        {
            var position = transform.position;
            position = Vector3.Lerp(position, movementVector, speed * Time.deltaTime);
            transform.position = position;
            movementVector = position;
        }
    }
}

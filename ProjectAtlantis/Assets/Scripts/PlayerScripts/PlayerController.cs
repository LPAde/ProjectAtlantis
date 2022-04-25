using Combat;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Movement")] 
        [SerializeField] private Vector3 movePos;
        
        #region Unity Methods

        private void Start()
        {
            movePos = transform.position;
        }
        
        private void Update()
        {
            CheckInputs();
        }
        
        private void FixedUpdate()
        {
            Move();
            Look();
        }

        #endregion
        

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var projectile = Instantiate(player.PlayerAttack, player.ProjectileSpawnPosition.position, quaternion.identity, player.transform).GetComponent<PlayerProjectile>();
                projectile.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), player.ProjectileSpawnPosition.forward);
            }

            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    var position = transform.position;
                    movePos = new Vector3(hit.point.x, position.y, hit.point.z);
                }
            }
        }

        /// <summary>
        /// Moves the player to the established position. (Will be done with the animator later.) 
        /// </summary>
        private void Move()
        {
            transform.position = Vector3.Lerp(transform.position, movePos, player.PlayerStats.Speed * Time.deltaTime);
        }

        /// <summary>
        /// Makes the player look into the direction of their mouse all the time.
        /// </summary>
        private void Look()
        {
            var lookAtPos = Input.mousePosition;
            lookAtPos.z = GameManager.Instance.MainCam.transform.position.y - transform.position.y;
            lookAtPos = GameManager.Instance.MainCam.ScreenToWorldPoint(lookAtPos);
            var transform1 = transform;
            transform1.forward = lookAtPos - transform1.position;
        }
    }
}

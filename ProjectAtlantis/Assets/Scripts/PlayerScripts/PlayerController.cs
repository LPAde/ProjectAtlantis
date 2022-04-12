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
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Vector3 movePos;
        [SerializeField] private float speed;

        public float Speed
        {
            get => speed;
            private set
            {
                speed = value;

                navMeshAgent.speed = Speed;
            }
        }

        #region Unity Methods

        private void Start()
        {
            movePos = transform.position;
            navMeshAgent.speed = Speed;
        }
        
        private void Update()
        {
            CheckInputs();
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        #endregion
        

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var projectile = Instantiate(player.PlayerAttack, player.ProjectileSpawnPosition.position, quaternion.identity, player.transform).GetComponent<PlayerProjectile>();
                projectile.Initialize(GameManager.Instance.RhythmManager.CheckTiming(), player.ProjectileSpawnPosition.forward);
            }

            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) {
                    movePos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
        }

        /// <summary>
        /// Moves the player to the established position. (Will be done with the animator later.) 
        /// </summary>
        private void Move()
        {
            navMeshAgent.SetDestination(movePos);
        }
    }
}

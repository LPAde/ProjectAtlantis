using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Movement")] 
        [SerializeField] private Vector3 movePos;
        [SerializeField] private Rigidbody rb;

        [Header("Dashes")]
        [SerializeField] private Vector3 dashVector;
        [SerializeField] private bool isDashing;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashSpeed;
        
        #region Unity Methods

        private void Awake()
        {
            GameManager.Instance.Load += Load;
            GameManager.Instance.Save += Save;
        }

        private void Start()
        {
            movePos = transform.position;
        }
        
        private void Update()
        {
            CheckInputs();
            Look(); 
        }

        private void FixedUpdate()
        {
            if (!isDashing)
            {
                Move();
            }
            else
            {
                Dash();
            }
        }

        #endregion

        #region Private Methods

        private void CheckInputs()
        {
            // Not the most attractive solution, but a simple one that works.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Cast(KeyCode.Q);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Cast(KeyCode.W);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Cast(KeyCode.E);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Cast(KeyCode.Space);
            }

            if (Input.GetMouseButton(1))
            {
                Ray ray = GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if(!hit.transform.CompareTag("Ground"))
                        return;

                    movePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }
            }
        }

        /// <summary>
        /// Moves the player to the established position. (Will be done with the animator later.) 
        /// </summary>
        private void Move()
        {
            // Find the target position relative to the player.
            var dir = movePos - transform.position;
            
            // Calculate movement at the desired speed.
            var movement = dir.normalized * (player.PlayerStats.Speed * Time.deltaTime);
            
            // Limit movement to never pass the target position.
            if (movement.magnitude > dir.magnitude) 
                movement = dir;

            movement += Physics.gravity * Time.deltaTime;
            
            // Move the character.
            player.CharacterController.Move(movement);
        }

        /// <summary>
        /// Makes the player look into the direction of their mouse all the time.
        /// </summary>
        private void Look()
        {
            // Converting the mouse position into a Vector the player can look to.
            var lookAtPos = Input.mousePosition;
            lookAtPos.z = GameManager.Instance.MainCam.transform.position.y - transform.position.y;
            lookAtPos = GameManager.Instance.MainCam.ScreenToWorldPoint(lookAtPos);
            var transform1 = transform;
            var position = transform1.position;
            
            // Small fixes I had to do.
            lookAtPos.y = position.y;
            lookAtPos.z -= 2;
            
            // Actually looking to the position.
            transform1.LookAt(lookAtPos);
        }

        private void Dash()
        {
            // Calculate movement at the desired speed.
            var movement = dashVector.normalized * (Time.deltaTime * dashSpeed);
                
            // Limit movement to never pass the target position.
            if (movement.magnitude > dashVector.magnitude)
                movement = dashVector;
                
            // Move the character.
            player.CharacterController.Move(movement);

            dashDuration -= Time.deltaTime;
            
            if (dashDuration > 0)
                return;

            movePos = transform.position;
            isDashing = false;
        }

        /// <summary>
        /// Casts a spell based on the input.
        /// </summary>
        /// <param name="code"> The Input that should be worked with. </param>
        private void Cast(KeyCode code)
        {
            switch (code)
            {
                case KeyCode.Q:
                    player.CombatSpells[0].Cast();
                    break;
                
                case KeyCode.W:
                    player.CombatSpells[1].Cast();
                    break;
                
                case KeyCode.E:
                    player.CombatSpells[2].Cast();
                    break;
                
                case KeyCode.Space: 
                    player.MovementSpell.Cast();
                    break;
            }
        }

        /// <summary>
        /// Reads the position of the player object.
        /// </summary>
        private void Load()
        {
            var vector = SaveSystem.GetVector3("PlayerPosition");
            
            if(vector == null || vector == Vector3.zero)
                return;
            
            transform.position = vector;
        }

        /// <summary>
        /// Saves the position of the player object.
        /// </summary>
        private void Save()
        {
            SaveSystem.SetVector3("PlayerPosition", transform.position);
        }

        #endregion

        public void InitializeDash(Vector3 newDashVector, float newDashDuration, float newDashSpeed)
        {
            dashVector = newDashVector;
            dashDuration = newDashDuration;
            dashSpeed = newDashSpeed;
            isDashing = true;
        }
    }
}
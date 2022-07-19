using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Movement")] 
        [SerializeField] private Vector3 movePos;
        [SerializeField] private bool isInAnimation;

        [Header("Dashes")]
        [SerializeField] private Vector3 dashVector;
        [SerializeField] private bool isDashing;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashSpeed;
        
        private static readonly int StartMoving = Animator.StringToHash("StartMoving");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int PressAttack = Animator.StringToHash("PressAttack");
        private static readonly int PressAttack2 = Animator.StringToHash("PressAttack2");

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
            if(isInAnimation)
                return;
            
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
                player.Anim.SetBool(IsMoving, true);
                player.Anim.SetTrigger(StartMoving);
            }

            // Move
            if (Input.GetMouseButton(1))
            {
                Ray ray = GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if(!hit.transform.CompareTag("Ground"))
                        return;

                    movePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    
                    Look(); 
                    player.Anim.SetBool(IsMoving, true);
                    player.Anim.SetTrigger(StartMoving);
                }
            }
            
            // Pause
            if(Input.GetKeyDown(KeyCode.Escape))
                GameManager.Instance.ToggleWindows();
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
            
            // Stop movement animation when there is no movement.
            if(movement.x == 0 && movement.z == 0)
                player.Anim.SetBool(IsMoving, false);
        }

        /// <summary>
        /// Makes the player look into the direction of their mouse all the time.
        /// </summary>
        private void Look()
        {
            var ray = GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit))
                return;
            
            var lookAtPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                
            transform.LookAt(lookAtPos);
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
                
            Look(); 
        }

        /// <summary>
        /// Casts a spell based on the input.
        /// </summary>
        /// <param name="code"> The Input that should be worked with. </param>
        private void Cast(KeyCode code)
        {
            Look(); 
            
            switch (code)
            {
                case KeyCode.Q:
                    DoCorrectAttackAnimation();
                    player.CombatSpells[0].Cast();
                    break;
                
                case KeyCode.W:
                    DoCorrectAttackAnimation();
                    player.CombatSpells[1].Cast();
                    break;
                
                case KeyCode.E:
                    DoCorrectAttackAnimation();
                    player.CombatSpells[2].Cast();
                    break;
                
                case KeyCode.Space: 
                    player.MovementSpell.Cast();
                    break;
            }
        }

        private void DoCorrectAttackAnimation()
        {
            if (player.Anim.GetCurrentAnimatorStateInfo(0).IsName("Eir_Hit_1") && !player.Anim.GetBool(PressAttack2))
            {
                player.Anim.SetBool(PressAttack2, true);
            }
            else
            {
                if (player.Anim.GetCurrentAnimatorStateInfo(0).IsName("Eir_Hit_2"))
                {
                    player.Anim.SetBool("PressAttack3", true);
                }
                else
                {
                    player.Anim.SetTrigger(PressAttack);
                }
            }
        }
        
        /// <summary>
        /// Reads the position of the player object.
        /// </summary>
        private void Load()
        {
            var vector = SaveSystem.GetVector3("PlayerPosition");
            
            if(vector == Vector3.zero)
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
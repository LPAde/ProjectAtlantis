using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Movement")] 
        [SerializeField] private Vector3 movePos;
        [SerializeField] private bool isInAnimation;

        [Header("Dashing")]
        [SerializeField] private Vector3 dashVector;
        [SerializeField] private bool isDashing;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashSpeed;

        [Header("Audio")] 
        [SerializeField] private AudioClip swimSound;
        
        [Header("Animation")]
        [SerializeField] private int attackAnimationIndex;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

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
                    
                    if(player.AudioSource.clip == swimSound)
                        return;
                    
                    player.AudioSource.clip = swimSound;
                    player.AudioSource.loop = true;
                    player.AudioSource.Play();
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
            if (movement.x == 0 && movement.z == 0)
            {
                player.Anim.SetBool(IsMoving, false);
                player.AudioSource.clip = null;  
            }
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
            byte spellIndex = 0;
            switch (code)
            {
                case KeyCode.Q:
                    DoCorrectAttackAnimation();
                    spellIndex = 0;
                    player.CombatSpells[spellIndex].Cast();
                    break;
                
                case KeyCode.W:
                    DoCorrectAttackAnimation();
                    spellIndex = 1;
                    player.CombatSpells[spellIndex].Cast();
                    break;
                
                case KeyCode.E:
                    DoCorrectAttackAnimation();
                    spellIndex = 2;
                    player.CombatSpells[spellIndex].Cast();
                    break;
                
                case KeyCode.Space:
                    spellIndex = 3;
                    player.MovementSpell.Cast();
                    break;
            }
            
            GameManager.Instance.HudManager.UseSkill(spellIndex);
        }

        private void DoCorrectAttackAnimation()
        {
            if (attackAnimationIndex == 0)
            {
                attackAnimationIndex++;
            }
            else
            {
                attackAnimationIndex = 0;
            }

            player.Anim.SetTrigger(string.Concat("PressAttack", attackAnimationIndex));
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
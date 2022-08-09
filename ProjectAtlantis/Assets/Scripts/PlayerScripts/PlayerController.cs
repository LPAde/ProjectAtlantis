using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Movement")] 
        [SerializeField] private Vector3 movePos;
        [SerializeField] private bool isInAnimation;
        [SerializeField] private MovementIndicator indicator;

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

                    if (player.LoopingAudioSource.clip != swimSound)
                    {
                        player.PlayLoopingSound(PlayerSounds.Swimming);
                        player.PlayOneTimeSound(PlayerSounds.StartSwimming);
                    }
                }
            }
            
            // Movement indicator on click.
            if(Input.GetMouseButtonDown(1))
                indicator.Activate(movePos);
            
            // Pause
            if (Input.GetKeyDown(KeyCode.Escape))
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
                player.StopLoopingSound();
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
            byte spellIndex = 5;
            Look();
            
            switch (code)
            {
                case KeyCode.Q:
                    if (player.CombatSpells[0].Cast())
                    {
                        player.PlayOneTimeSound(PlayerSounds.Attack);
                        DoCorrectAttackAnimation(); 
                        spellIndex = 0;
                    }
                    break;
                
                case KeyCode.W:
                    
                    if (player.CombatSpells[1].Cast())
                    {
                        player.PlayOneTimeSound(PlayerSounds.Attack);
                        DoCorrectAttackAnimation();
                        spellIndex = 1;
                    }
                    break;
                
                case KeyCode.E:
                    
                    if (player.CombatSpells[2].Cast())
                    {
                        player.PlayOneTimeSound(PlayerSounds.Attack2);
                        DoCorrectAttackAnimation();
                        spellIndex = 2;
                    }
                    break;
                
                case KeyCode.Space:
                    if (player.MovementSpell.Cast())
                    {
                        spellIndex = 3;
                        player.Anim.SetBool(IsMoving, true);
                        player.PlayOneTimeSound(PlayerSounds.StartSwimming);
                    }
                    break;
            }
            
            if(spellIndex < 5) 
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

        #endregion

        public void LookAtMovement()
        {
            transform.LookAt(new Vector3(movePos.x, transform.position.y, movePos.z));
        }
        
        public void InitializeDash(Vector3 newDashVector, float newDashDuration, float newDashSpeed)
        {
            dashVector = newDashVector;
            dashDuration = newDashDuration;
            dashSpeed = newDashSpeed;
            isDashing = true;
        }
    }
}
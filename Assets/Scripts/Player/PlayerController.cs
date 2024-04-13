using System.Collections;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalMedia
{
    public class PlayerController : CoreCharacter
    {
        //Movement 
        private PlayerInput _playerInput;
        private InputAction move;
        private InputAction jump;
        private InputAction dodge;
        private InputAction placeholder; // Use this as needed and add more. 

        private Rigidbody2D rb;
        #region Wall Jumping and Sliding

        private bool canWallJump = true;
        private bool isWallSliding;
        private bool jumpLeft, jumpRight; 
        private float wallSlidingSpeed = 2f;
        private float wallJumpingDirection;
        private float wallJumpingCounter;
        private bool shouldCheckForLanding = false;

        #endregion
        
        //Animation States 
        private const string PLAYER_WALK = "Player_Walk_Start";
        
        //Add above values as needed. 
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            move = _playerInput.actions["Move"];
            jump = _playerInput.actions["Jump"];
            jump.performed += Jump;
            
            // dodge = _playerInput.actions["Dodge"];   
            
            _animator = GetComponent<Animator>();
        }
       
        private void FixedUpdate()
        {
            Move();
            if (currentState == State.Airborne && shouldCheckForLanding)
            {
                if (IsGrounded())
                {
                    _animator.Play("Player_Jump-End");
                    InitiateStateChange(State.Idle);
                    shouldCheckForLanding = false;
                }
            }
           
        }
        
        private void Jump(InputAction.CallbackContext context)
        {
            //Debug.Log("try to jump");
            //Make your jump here. 
            if(currentState == State.Attacking)
                return;
            
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, data.BasicData.jumpingStrength);
               // InitiateStateChange(State.Airborne);
                _animator.Play("Player_Jumping");
                InitiateStateChange(State.Airborne);
                StartCoroutine(JumpCheckDelay());
            }
            else if (canWallJump)
            {
                //InitiateStateChange(State.Airborne);
                canWallJump = false;
                if (jumpLeft)
                {
                    rb.velocity = new Vector2(-550, data.BasicData.jumpingStrength);
                }
                else if (jumpRight)
                {
                    rb.velocity = new Vector2(15, data.BasicData.jumpingStrength);
                }
            }
            /*else if (canDoubleJump && rb.velocity.y != 0)
            {
                InitiateStateChange(State.Airborne);
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, data.BasicData.jumpingStrength);
            }*/
            
        }
        private bool IsGrounded()
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, data.BasicData.jumpDistanceCheck, groundLayer))
            {
                //canDoubleJump = true;
                return true;
            }

            return false;
        }

        private IEnumerator JumpCheckDelay()
        {
            yield return new WaitForSeconds(.5f);

            shouldCheckForLanding = true;
        }
        
        #region Wall Sliding 

        private bool IsWalled()
        {
            //Split this to an individual left and right check. 
            if (Physics2D.Raycast(transform.position, Vector2.left, 1f, groundLayer))
            {
                jumpLeft = true; 
                return true;
            }
            else if (Physics2D.Raycast(transform.position, Vector2.right, 1f, groundLayer))
            {
                jumpRight = true; 
                return true;
            }

            jumpRight = false;
            jumpLeft = false; 
            
            return false;
        }
        
        private void WallSlide()
        {
            if (IsWalled() && !IsGrounded() && rb.velocity.x != 0)
            {
                canWallJump = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                canWallJump = false;
            }
        }

        #endregion

        /// <summary>
        /// I may update this later to only trigger when the player presses a key, as right now it is quite an expensive operation. 
        /// </summary>
        private void Move()
        {
            //Check if the player was moving and if so set their velocity (x) back to 0 and return. 
            if(currentState is State.Attacking or State.Deathblowing or State.Staggered or State.Dashing)
            {
                return;
            }
            else
            {
                Vector2 moveDirection = move.ReadValue<Vector2>();
                Vector2 playerVelocity = new Vector2(moveDirection.x * data.BasicData.speed, rb.velocity.y);
                rb.velocity = playerVelocity;
            
                if (playerVelocity.x > 0)
                {
                    WallSlide();
                    //InitiateStateChange(State.Moving);
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                    if(currentState is not State.Airborne) _animator.Play(PLAYER_WALK);
                }
                else if (playerVelocity.x < 0)
                {
                    WallSlide();
                    //InitiateStateChange(State.Moving);
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    if(currentState is not State.Airborne) _animator.Play(PLAYER_WALK);
                }
                else
                {
                    canWallJump = false;
                    if (currentState is State.Blocking or State.Deathblowing or State.Staggered) return;
                    _animator.Play("Idle");
                }
            }
        }
    }
}

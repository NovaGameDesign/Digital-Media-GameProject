using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DigitalMedia
{
    public class PlayerController : CoreCharacter
    {
        //Movement 
        private PlayerInput _playerInput;
        private InputAction move;
        private InputAction jump;
        private InputAction dodge;
        private InputAction reload;
        private InputAction placeholder; // Use this as needed and add more. 

        private Rigidbody2D rb;
        private bool canDoubleJump = true;
     
        private string currentAnimState;

        #region wall sliding
        
        private bool canWallJump = true;
        private bool isWallSliding;
        private float wallSlidingSpeed = 2f;
        private float wallJumpingDirection;
        private float wallJumpingTime = 0.2f;
        private float wallJumpingCounter;
        private float wallJumpingDuration = 0.4f;
        private Vector2 wallJumpingPower = new Vector2(8f, 16f);

        #endregion
  
        
        //Animation States 
        private const string PLAYER_IDLE = "Idle";
        private const string PLAYER_WALK = "Walk";
        private const string PLAYER_RUN = "Player_Run";
        private const string PLAYER_JUMP = "Player_Jump";
        //Add above values as needed. 
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            move = _playerInput.actions["Move"];
            jump = _playerInput.actions["Jump"];
            jump.performed += Jump;
            reload = _playerInput.actions["Reload"];
            reload.performed += reloadScene;
            
            // dodge = _playerInput.actions["Dodge"];   
            
            _animator = GetComponent<Animator>();
        }

        
       
        private void FixedUpdate()
        {
            Move();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            //Debug.Log("try to jump");
            //Make your jump here. 
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, data.BasicData.jumpingStrength);
            }
            else if (canDoubleJump && rb.velocity.y != 0)
            {
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, data.BasicData.jumpingStrength);
            }
        }
        
        #region Wall Jumping and Sliding 

        private bool IsWalled()
        {

            if (Physics2D.Raycast(transform.position, Vector2.left, .5f, groundLayer) || Physics2D.Raycast(transform.position, Vector2.right, .5f, groundLayer))
            {
                canWallJump = true;
                canDoubleJump = true;
                return true;
            }

            return false;
        }
        private void WallSlide()
        {
            if (IsWalled() && !IsGrounded() && rb.velocity.x != 0)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }

            if (IsWalled())
            {
                wallJumpingDirection = -transform.localScale.x;
                rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            }
            else if (canWallJump && rb.velocity.y > 0 && (rb.velocity.x>0 || rb.velocity.x < 0))
            {
                canWallJump = false;
            }

            if (IsWalled())
            {
                wallJumpingDirection = -transform.localScale.x;
                rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            }
            else if (canWallJump && rb.velocity.y > 0 && (rb.velocity.x>0 || rb.velocity.x < 0))
            {
                canWallJump = false;
            }
        }
       

        #endregion
        
        
        private void reloadScene(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene("Main");
        }

        private bool IsGrounded()
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, data.BasicData.jumpDistanceCheck, groundLayer))
            {
                canDoubleJump = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// I may update this later to only trigger when the player presses a key, as right now it is quite an expensive operation. 
        /// </summary>
        private void Move()
        {
            Vector2 moveDirection = move.ReadValue<Vector2>();
            
            if(currentState != State.Idle)
                return;
            
            
            Vector2 playerVelocity = new Vector2(moveDirection.x * data.BasicData.speed, rb.velocity.y);
            rb.velocity = playerVelocity;
            if (playerVelocity.x > 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
                ChangeAnimationState(PLAYER_WALK);
            }
            else if (playerVelocity.x < 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                ChangeAnimationState(PLAYER_WALK);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }
        
        private void ChangeAnimationState(string newState)
        {
            //Checks if the animation is already playing 
            if (newState == currentAnimState) return;

            //Plays a new animation
            _animator.Play(newState);

            //Sets the current animation for later use. 
            currentAnimState = newState;
        }

    }
}

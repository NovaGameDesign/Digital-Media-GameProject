using System;
using System.Collections;
using DigitalMedia.Core;
using DigitalMedia.Misc;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static DigitalMedia.Core.State;

namespace DigitalMedia
{
    public class PlayerController : CoreCharacter
    {
        //Movement 
        private PlayerInput _playerInput;
        public InputAction move;
        private InputAction jump;
        private InputAction dodge;
        private InputAction dash;
        
        private Rigidbody2D rb;

        private bool moving;
        
        #region Wall Jumping and Sliding
        
        [System.NonSerialized] public bool canWallJump = true;
        private bool isWallSliding;
        private bool jumpLeft, jumpRight; 
        private float wallSlidingSpeed = 2f;
        private float wallJumpingDirection;
        private float wallJumpingCounter;
        private bool shouldCheckForLanding = false;

        #endregion
        
        //Animation States 
        private const string PLAYER_WALK = "Player_Walk_Start";
        
        //Dashing
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashSpeed;

        private float dashCooldownTimeLeft;
        [SerializeField] private float dashCooldown;
        private float dashCooldownStartTime;
        private bool dashOnCooldown = false;

        public float lastImageXpos;

        public float distanceBetweenTwoImages;

        private RaycastHit2D dashHitPosition;
        private Vector2 dashTarget;
        private Vector2 dashStartPosition;
        private float distanceToTarget;

        
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            move = _playerInput.actions["Move"];
            jump = _playerInput.actions["Jump"];
            dash = _playerInput.actions["Dash"];
            jump.performed += Jump;
            dash.performed += StartDash;
            move.performed += IsMoving;
            move.canceled += IsMoving;
            
            // dodge = _playerInput.actions["Dodge"];   
            
            _animator = GetComponent<Animator>();
        }
       
        private void FixedUpdate()
        {
            if(moving) Move();
            if (currentState == Dashing)
            {
                CheckDash();
            }
            if (currentState == Airborne && shouldCheckForLanding)
            {
                if (IsGrounded())
                {
                    _animator.Play("Player_Jump-End");
                    InitiateStateChange(Idle);
                    shouldCheckForLanding = false;
                }
            }
           
        }

        #region Jumping
        
        private void Jump(InputAction.CallbackContext context)
        {
            //Debug.Log("try to jump");
            //Make your jump here. 
            if(currentState == Attacking)
                return;
            
            if (IsGrounded())
            {
                InitiateStateChange(Airborne);
                rb.velocity = new Vector2(rb.velocity.x, data.BasicData.jumpingStrength);
                // InitiateStateChange(State.Airborne);
                _animator.Play("Player_Jumping");
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
        public bool IsGrounded()
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
        
        #endregion
       
        
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
        
        
        #region Dashing
        
        private void StartDash(InputAction.CallbackContext context)
        {

            dashOnCooldown = true;
            dashCooldownStartTime = Time.time;
            
            //Play an animation and begin moving forward. 
            _animator.Play("Player_Dash-Default");
            InitiateStateChange(Dashing);

            
            dashStartPosition = transform.position;
            if (transform.rotation.y > 0)
            {
                dashHitPosition  = Physics2D.Raycast(transform.position, transform.right, dashDistance, groundLayer);
            }
            else if (transform.rotation.y <= 0)
            {
                dashHitPosition  = Physics2D.Raycast(transform.position, -transform.right, dashDistance, groundLayer);
            }
            
            distanceToTarget = dashHitPosition.distance;
            if (dashHitPosition.collider != null) //If we did hit something.
            {
                dashTarget = dashHitPosition.point;
                Debug.Log($"We hit a wall or object, the hit location was {dashHitPosition.point}");
            }
            else
            {
                float leftRight = transform.rotation.y > 0 ? 1 : -1;
                dashTarget = new Vector2(transform.position.x + leftRight * dashDistance, transform.position.y);
                Debug.Log($"We did not hit anything when trying to dash, the current targeted end point is: {dashTarget}");
            }

            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Boss");
            

            dashHitPosition = new RaycastHit2D();
        }

        private void CheckDash()
        {
            float leftRight = transform.rotation.y > 0 ? 1 : -1;
            rb.velocity = new Vector2(dashSpeed * leftRight, 0);

            if (Vector2.Distance(transform.position, dashTarget) <= 1)
            {
                Debug.Log($"Canceled dash execution at {transform.position} position");
                rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0; 
                InitiateStateChange(Idle);
                _animator.Play("Idle");
                return;
            }
            
            /*if (transform.position.x + distanceToTarget >= dashTarget.x - 0.25f && leftRight == 1)
            {
                rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0; 
                InitiateStateChange(Idle);
                _animator.Play("Idle");
            }
            else if (transform.position.x - distanceToTarget >= dashTarget.x + 0.25f && leftRight == -1)
            {
                rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0; 
                InitiateStateChange(Idle);
                _animator.Play("Idle");
            }*/
          
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenTwoImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }

   

        #endregion

        private void IsMoving(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                moving = true;
            }
            else if (context.canceled)
            {
                moving = false;
                canWallJump = false;
                if (currentState is Blocking or Deathblowing or Staggered or Attacking) return;
                _animator.Play("Idle");
            }
        }
        /// <summary>
        /// I may update this later to only trigger when the player presses a key, as right now it is quite an expensive operation. 
        /// </summary>
        private void Move()
        {
            //Check if the player was moving and if so set their velocity (x) back to 0 and return. \
    
            if(currentState is Idle or Airborne)
            {
                Vector2 moveDirection = move.ReadValue<Vector2>();
                Vector2 playerVelocity = new Vector2(moveDirection.x * data.BasicData.speed, rb.velocity.y);
                
                rb.velocity = playerVelocity;
            
                if (playerVelocity.x > 0)
                {
                    WallSlide();
                    //InitiateStateChange(State.Moving);
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                    if(currentState is not Airborne) _animator.Play(PLAYER_WALK);
                }
                else if (playerVelocity.x < 0)
                {
                    WallSlide();
                    //InitiateStateChange(State.Moving);
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    if(currentState is not Airborne) _animator.Play(PLAYER_WALK);
                }
                else
                {
                    canWallJump = false;
                    if (currentState is Blocking or Deathblowing or Staggered) return;
                    _animator.Play("Idle");
                }
            }
            else
            {
              return;
            }
        }
    }
}

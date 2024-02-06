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
        private InputAction dodge;
        private InputAction placeholder; // Use this as needed and add more. 

        private Rigidbody2D rb;

        [SerializeField] float playerSpeed;

        private Animator _animator;

        private string currentAnimState;
        //Animation States 
        private const string PLAYER_IDLE = "Player_Idle";
        private const string PLAYER_WALK = "Player_Walk";
        private const string PLAYER_RUN = "Player_Run";
        private const string PLAYER_JUMP = "Player_Jump";
        //Add above values as needed. 
    
    
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            move = _playerInput.actions["Move"];
            // dodge = _playerInput.actions["Dodge"];   
        }

        private void FixedUpdate()
        {
            Jump();
            Move();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void Jump()
        {
            //Make your jump here. 
        }

        /// <summary>
        /// I may update this later to only trigger when the player presses a key, as right now it is quite an expensive operation. 
        /// </summary>
        private void Move()
        {
            Vector2 moveDirection = move.ReadValue<Vector2>();
            Vector2 playerVelocity = new Vector2(moveDirection.x * playerSpeed, rb.velocity.y);
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

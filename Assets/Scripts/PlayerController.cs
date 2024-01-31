using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    //Movement 
    private PlayerInput _playerInput;
    private InputAction move;
    private InputAction dodge;
    private InputAction placeholder; // Use this as needed and add more. 

    private Rigidbody2D rb;

    [SerializeField] float playerSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        move = _playerInput.actions["Move"];   
        dodge = _playerInput.actions["Dodge"];   
        
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
    private void Move()
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        Vector2 playerVelocity = new Vector2(moveDirection.x * playerSpeed, rb.velocity.y);
        rb.velocity = playerVelocity; 
    }
}

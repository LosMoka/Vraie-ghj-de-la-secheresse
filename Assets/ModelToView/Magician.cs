using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rigidBody;
    private Vector3 velocity = Vector3.zero;
    private Model.Player playerBehaviour = new Model.Player(new Vector2(0,0));
    private bool isJumping;
    private bool isGrounded;
    private bool isWallJump;
    public Animator animator;
    public Transform groundCheck;
    public Transform wallJumpCheckA;
    public Vector3 wallJumpAreaSize = new Vector3(20, 10, 10);
    public float groundCheckRadius;
    public LayerMask collisionLayer;
    public SpriteRenderer spriteRenderer;
    private Model.Vector2i vecb; 
    private float horizontalMovement;
    private Model.Point2i dest;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed* Time.deltaTime;
        velocity.x = horizontalMovement;
        velocity.y = rigidBody.velocity.y;


        if (Input.GetButtonDown("Jump") && isGrounded)
            isJumping = true;

        if (isJumping)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }

        playerBehaviour.movePlayer(velocity);      
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, playerBehaviour.Position, ref velocity, 0.05f);

        flip(rigidBody.velocity.x);
        float characterVelocity = Mathf.Abs(rigidBody.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);
        isWallJump = Physics2D.OverlapBox(wallJumpCheckA.position, wallJumpAreaSize, 0f);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.blue;
        
    }

    void flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}

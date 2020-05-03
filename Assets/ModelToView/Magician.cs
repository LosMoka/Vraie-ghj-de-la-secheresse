using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Magician : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rigidBody;
    private Vector3 velocity = Vector3.zero;
    private Model.Player playerBehaviour = new Model.Player(new Vector2(0, 0));
    private bool isJumping;
    private bool isGrounded;
    public bool isWallJump;
    public Animator animator;
    public Transform groundCheck;
    public Transform wallJumpCheckA;

    public float groundCheckRadius;
    public float groundCheckRadius2;
    public LayerMask collisionLayer;
    public SpriteRenderer spriteRenderer;
    private float horizontalMovement;

    private bool isFireballReady = true;


    private bool isInvicible;

    public GameObject fireball;

    // Start is called before the first frame update 
    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 100;
        velocity.x = horizontalMovement;
        velocity.y = rigidBody.velocity.y;


        if (Input.GetButtonDown("Jump") && isGrounded)
            isJumping = true;

        if (isJumping)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));      
            isJumping = false;
        }

        if (isWallJump && Input.GetButtonDown("Jump") && !isGrounded)
        {           
            rigidBody.AddRelativeForce(new Vector2(-1000f, jumpForce));        
        }

        if (Input.GetButtonDown("Jump") && isFireballReady)
        {
            isFireballReady = false;
            animator.SetBool("ThrowFireBall", true);
            StartCoroutine(fireballDelay());
            
        }

        playerBehaviour.movePlayer(velocity);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, playerBehaviour.Position, ref velocity, 0.02f);

        flip(rigidBody.velocity.x);
        float characterVelocity = Mathf.Abs(rigidBody.velocity.x);
        animator.SetFloat("Speed", characterVelocity);

       // transform.position = rigidBody.position;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);
        isWallJump = Physics2D.OverlapBox(wallJumpCheckA.position, new Vector2(0.5f, 0.4f), 0, collisionLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyAttack"))
        {
            playerBehaviour.hit(Model.Player.HitType.NORMAL);
            isInvicible = true;
            StartCoroutine("invisibilityFlash");
        }
    }


    public void throwFireBall()
    {       
        Instantiate(fireball, this.transform);       
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallJumpCheckA.position, new Vector2(0.5f,0.4f));

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

    public IEnumerator invisibilityFlash()
    {
        StartCoroutine("HandleInvicibilityDelay");

        while (isInvicible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);

        }
    }

    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(3);
        isInvicible = false;
    }

    public IEnumerator fireballDelay()
    {
        
        yield return new WaitForSeconds(0.3f);
        throwFireBall();
        yield return new WaitForSeconds(0.7f);
        isFireballReady = true;
        animator.SetBool("ThrowFireBall", false);
        

    }
}

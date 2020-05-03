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
    public bool isGrounded;
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
    private bool isFireAttackReady = true;

    private bool isWallJumpDelay = true;
    private bool isInvicible;

    public GameObject fireball;
    public GameObject fireAttack;

    enum direction
    {
        RIGHTDIR,
        LEFTDIR
    };

    private direction playerDirection;

    // Update is called once per frame 
    void Update()
    {

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 100;
        velocity.x = horizontalMovement;
        velocity.y = rigidBody.velocity.y;

        if (horizontalMovement >= 0)
            playerDirection = direction.RIGHTDIR;
        else playerDirection = direction.LEFTDIR;

        if (Input.GetButtonDown("Jump") && isGrounded)
            isJumping = true;

        if (isJumping)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));      
            isJumping = false;
        }

        if (isWallJump && Input.GetButtonDown("Jump") && !isGrounded)
        {
            wallJump();            
        }

        if (!isWallJumpDelay)
        {
            if (playerDirection == direction.LEFTDIR)
                rigidBody.AddRelativeForce(new Vector2(+Time.deltaTime * 500 * 50, 0));

            else if (playerDirection == direction.RIGHTDIR)
                rigidBody.AddRelativeForce(new Vector2(-Time.deltaTime * 500 * 50, 0));
        }

        if (Input.GetButtonDown("Fire1") && isFireballReady)
        {
            isFireballReady = false;
            animator.SetBool("ThrowFireBall", true);
            StartCoroutine(fireballDelay());     
        }

        if (Input.GetButtonDown("Fire2") && isFireAttackReady)
        {
            isFireAttackReady = false;
            animator.SetBool("ThrowFireBall", true);
            StartCoroutine(fireAttackDelay());
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

    public void wallJump()
    {
        isWallJumpDelay = false;
        StartCoroutine(WallJumpDelay());

        if (playerDirection == direction.LEFTDIR)
            rigidBody.AddRelativeForce(new Vector2(0f, 250));

        else if (playerDirection == direction.RIGHTDIR)
            rigidBody.AddRelativeForce(new Vector2(0f, 250));
    }
    public void throwFireAttack()
    {
        Instantiate(fireAttack, this.transform);
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

    public IEnumerator WallJumpDelay()
    {
        yield return new WaitForSeconds(0.1f);
        isWallJumpDelay = true;
    }
    public IEnumerator fireballDelay()
    {  
        yield return new WaitForSeconds(0.3f);
        throwFireBall();
        yield return new WaitForSeconds(0.7f);
        isFireballReady = true;
        animator.SetBool("ThrowFireBall", false);
    }

    public IEnumerator fireAttackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        throwFireAttack();
        yield return new WaitForSeconds(0.7f);
        isFireAttackReady = true;
        animator.SetBool("ThrowFireBall", false);
    }
}

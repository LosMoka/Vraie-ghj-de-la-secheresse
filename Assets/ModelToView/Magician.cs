using System.Collections;
using System.Collections.Generic;
using Model;
using ModelToView;
using Network;
using UnityEngine;



public class Magician : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rigidBody;
    private Vector3 velocity = Vector3.zero;
    private Model.Player playerBehaviour;
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
    private Client m_client;
    private ClientUdp m_client_udp;
    private MapManager m_map_manager;
    private string map_as_string;
    public MapLoader mapLoader;


    enum direction
    {
        RIGHTDIR,
        LEFTDIR
    };

    private direction playerDirection;
    // Start is called before the first frame update  
    void Start()
    {
        GameObject gameManagerGameObject = GameObject.Find("GameManager");

        if (gameManagerGameObject != null)
        {
            GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();

            m_client = gameManager.Client;
            m_client_udp = gameManager.ClientUdp;
            playerBehaviour = gameManager.Player;
            m_map_manager = gameManager.MapManager;

            map_as_string = null;
            m_client.addNetworkMessageHandler("MAP", delegate (string data) { map_as_string = data; });
            m_client.send("GETMAP");
        }
        else
        {
            Debug.LogError("GameManager not found !");
            playerBehaviour = new Player(new Vector3(0, 0, 0));
            m_map_manager = new MapManager();
        }
    }
    void Update()
    {
 

        if (map_as_string != null)
        {
            m_map_manager.loadFromString(map_as_string);
            mapLoader.loadMap(m_map_manager);
            map_as_string = null;
        }


        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 100;
        velocity.x = horizontalMovement;
        velocity.y = rigidBody.velocity.y;

        if (horizontalMovement >= 0)
            playerDirection = direction.RIGHTDIR;
        else
            playerDirection = direction.LEFTDIR;

        if (Input.GetButtonDown("Jump") && isGrounded)
            isJumping = true;

        if (isJumping)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }

        if (isWallJump && Input.GetButtonDown("Jump") && !isGrounded)
        {
            if (playerDirection == direction.LEFTDIR)
                rigidBody.AddRelativeForce(new Vector2(+100, 300));

            else if (playerDirection == direction.RIGHTDIR)
                rigidBody.AddRelativeForce(new Vector2(-100, 300));
        
           // rigidBody.AddForce(new Vector2(0f, jumpForce));
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
        Gizmos.DrawWireCube(wallJumpCheckA.position, new Vector2(0.5f, 0.4f));

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

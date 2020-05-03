using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public float fireBallTimer;
    public float speed;
    public Animator animator;

    private bool isDestroyed = false;
    private bool isOnMovement = true;
    private float xPos, yPos;

    enum direction
     {
        RIGHTDIR,
        LEFTDIR
    };

    direction fireballDirection;
    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(FireBallTimer());
        
        transform.position = this.gameObject.transform.parent.position;
        xPos = this.gameObject.transform.parent.position.x;
        yPos = this.gameObject.transform.parent.position.y;

        if (this.gameObject.transform.parent.GetComponent<Magician>().spriteRenderer.flipX)
        {
            this.spriteRenderer.flipX = false;
            fireballDirection = direction.LEFTDIR;

        }
        else
        {
            this.spriteRenderer.flipX = true;
            fireballDirection = direction.RIGHTDIR;
        }

    }

    private void Update()
    {
        if (isOnMovement)
        {
            if (fireballDirection == direction.LEFTDIR)
            {
                this.gameObject.transform.position = new Vector3(xPos - 2.0f, yPos, 0);
                xPos -= Time.deltaTime * speed;
            }
            else
            {
                this.gameObject.transform.position = new Vector3(xPos + 2.0f, yPos, 0);
                xPos += Time.deltaTime * speed;
            }
        }
        else
        {
            if (fireballDirection == direction.LEFTDIR)
            {
                this.gameObject.transform.position = new Vector3(xPos - 3.0f, yPos, 0);
                
            }
            else
            {
                this.gameObject.transform.position = new Vector3(xPos + 3.0f, yPos, 0);
            }
        }
   
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isExplosed", true);
            StartCoroutine(Destruction());
            isOnMovement = false;
        }          
            
    }

    public IEnumerator FireBallTimer()
    {
        yield return new WaitForSeconds(fireBallTimer);
        StartCoroutine(Destruction());
        isDestroyed = true;
    }

    public IEnumerator Destruction()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public float fireBallTimer;
    public float speed;

    private bool isDestroyed = false;
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
        if ( fireballDirection == direction.LEFTDIR)
        {
            this.gameObject.transform.position = new Vector3(xPos - 2.0f, yPos, 0);
            xPos -= Time.deltaTime * speed;
        }
        else
        {
            this.gameObject.transform.position = new Vector3(xPos + .0f, yPos, 0);
            xPos += Time.deltaTime * speed;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Magician magician = collision.transform.GetComponent<Magician>();
 
        if (magician.spriteRenderer.flipX)
            this.spriteRenderer.flipX = true;
        else
            this.spriteRenderer.flipX = false;
*/
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
            
            //Magician enemy = collision.transform.GetComponent<Magician>();
            
    }




    public IEnumerator FireBallTimer()
    {
        yield return new WaitForSeconds(fireBallTimer);
        Destroy(gameObject);
        isDestroyed = true;


    }
}

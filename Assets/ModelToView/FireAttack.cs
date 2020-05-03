using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fireAttackTimer;

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
        StartCoroutine(FireAttackTimer());

        transform.position = this.gameObject.transform.parent.position;
        xPos = this.gameObject.transform.parent.position.x;
        yPos = this.gameObject.transform.parent.position.y;

        if (this.gameObject.transform.parent.GetComponent<Magician>().spriteRenderer.flipX)
        {
            fireballDirection = direction.LEFTDIR;

        }
        else
        {
            fireballDirection = direction.RIGHTDIR;
        }

    }

    private void Update()
    {
        if (fireballDirection == direction.LEFTDIR)
        {
            this.gameObject.transform.position = new Vector3(xPos - 2.0f, yPos - 0.2f, 0);            
        }
        else if (fireballDirection == direction.RIGHTDIR)
        {
            this.gameObject.transform.position = new Vector3(xPos + 2.0f, yPos - 0.2f, 0);          
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public IEnumerator FireAttackTimer()
    {
        yield return new WaitForSeconds(fireAttackTimer);
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public float damage = 1f;
    protected float dir;
    protected Vector3 velocity = new Vector3(0,0,0);
    private Rigidbody2D rb;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //get direction that player is facing

        //Debug.Log("new projectile!");
        //velocity.x *= speed;
        //transform.Translate(velocity * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
        //Debug.Log(rb.velocity);
        velocity.x = rb.velocity.x * speed;
        Physics2D.IgnoreLayerCollision(15, 13);
        Physics2D.IgnoreLayerCollision(15, 15);
    }

    protected virtual void collide(Collider2D collision)
    {
        GameObject contact = collision.gameObject;

        //can't hit player or other projectile
        //if (!contact.tag.Equals("Player") && contact.layer != 15)
        //{
        //Debug.Log("hit something");
        //Debug.Log("i shouldn't be here");
        if (contact.layer == 14)
        {
            
            Debug.Log("hit!");
            SimpleHostile pscript = contact.GetComponent<SimpleHostile>();
            //Debug.Log(lastDir);

            pscript.takeDamage(damage, dir);
            
        }

        if (contact.layer == 8)
            gameObject.SetActive(false);
//        }


            /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log(velocity);
        dir = Mathf.Sign(rb.velocity.x);
        transform.Translate(velocity * Time.deltaTime);
    }
}

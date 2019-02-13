using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    private float dir;
    Vector3 velocity = new Vector3(0,0,0);
    private Rigidbody2D rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //get direction that player is facing

        //Debug.Log("new projectile!");
        //velocity.x *= speed;
        //transform.Translate(velocity * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
        //Debug.Log(rb.velocity);
        velocity.x = rb.velocity.x * speed;
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject contact = collision.gameObject;

        //can't hit player or other projectile
        if (!contact.tag.Equals("Player") && contact.layer != 10)
        {
            //Debug.Log("hit something");
            if (contact.layer == 9)
            {
                Debug.Log("hit!");
                SimpleHostile pscript = contact.GetComponent<SimpleHostile>();
                //Debug.Log(lastDir);
                pscript.takeDamage(damage, dir);
            }

            //gameObject.SetActive(false);
        }


            /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(velocity);
        dir = Mathf.Sign(rb.velocity.x);
        transform.Translate(velocity * Time.deltaTime);
    }
}

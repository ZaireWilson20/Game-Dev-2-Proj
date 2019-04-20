using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    protected Vector2 direction;
    //protected float dir;
    protected Vector3 velocity = new Vector3(0, 0, 0);
    private Rigidbody2D rb;
    protected GameObject player;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //get direction that player is facing

        //Debug.Log("new projectile!");
        //velocity.x *= speed;
        //transform.Translate(velocity * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(rb.velocity);
        direction = Vector3.Normalize(player.transform.position - transform.position);
        velocity = direction * speed;
        Physics2D.IgnoreLayerCollision(16, 14);
        Physics2D.IgnoreLayerCollision(16, 16);
        Physics2D.IgnoreLayerCollision(16, 15);
    }

    protected virtual void collide(Collider2D collision)
    {
        GameObject contact = collision.gameObject;

        //can't hit player or other projectile
        //if (!contact.tag.Equals("Player") && contact.layer != 15)
        //{
        //Debug.Log("hit something");
        //Debug.Log("i shouldn't be here");
        if (contact.tag.Equals("Player"))
        {

            Debug.Log("hit!");
            Player pscript = contact.GetComponent<Player>();
            //Debug.Log(lastDir);

            pscript.takeDamage(damage, direction);
            Destroy(this.gameObject);
        }

        if (contact.layer == 14)
        {
            SimpleHostile hscript = contact.GetComponent<SimpleHostile>();
            hscript.takeDamage(damage, direction);
            Destroy(this.gameObject);
        }

        if (contact.layer == 18)
        {
            velocity *= -1;
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
        transform.Translate(velocity * Time.deltaTime);
    }
}
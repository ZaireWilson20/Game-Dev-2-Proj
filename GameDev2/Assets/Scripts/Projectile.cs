using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public float damage = 1f;
    protected Vector2 dir;
    protected Vector3 velocity = new Vector3(0,0,0);
    protected Rigidbody2D rb;
    protected Vector3 startPos;
    protected SpriteRenderer sprite;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //get direction that player is facing

        //Debug.Log("new projectile!");
        //velocity.x *= speed;
        //transform.Translate(velocity * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
        dir = rb.velocity;
        //Debug.Log(rb.velocity);
        velocity = rb.velocity * speed;
        Physics2D.IgnoreLayerCollision(15, 13);
        Physics2D.IgnoreLayerCollision(15, 15);
        startPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        Debug.Log(velocity);
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

            SimpleHostile hscript;
            FactoryBoss fscript;
            Debug.Log("hit!");
            if (contact.tag.Equals("FactoryBoss"))
            {
                fscript = contact.GetComponent<FactoryBoss>();
                fscript.takeDamage(damage, dir);
            }
            else
            {
                hscript = contact.GetComponent<SimpleHostile>();
                hscript.takeDamage(damage, dir);
            }
            //Debug.Log(lastDir);

            //pscript.takeDamage(damage, dir);

            if (contact.layer == 8)
                gameObject.SetActive(false);
//        }


            /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");*/


        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log(velocity);
        transform.Translate(velocity * Time.deltaTime);
    }
}

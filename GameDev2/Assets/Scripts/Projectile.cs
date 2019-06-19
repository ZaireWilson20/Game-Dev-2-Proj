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

    //public GameObject gameManagerObj;
    //protected GameState gameManager;
    //protected bool justPaused = false;
    //protected Vector2 storeVel;

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
        //Debug.Log(velocity);

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
            SalBoss salScript;
            Debug.Log("hit!");
            if (contact.tag.Contains("Mantis"))
            {
                fscript = contact.transform.parent.gameObject.GetComponent<FactoryBoss>();
                if (contact.tag.Contains("Head"))
                {
                    //double damage for headshot
                    Debug.Log("Headshot");
                    fscript.takeDamage(2 * damage, dir);
                } else if (contact.tag.Equals("Claws"))
                {
                    //regular damage for claws
                    fscript.takeDamage(damage, dir);
                } else
                {
                    //legs can be shielded
                    if (!fscript.shielded)
                        fscript.takeDamage(damage, dir);
                }
                //fscript.takeDamage(damage, dir);
            }
            else if (contact.tag.Contains("Sal"))
            {
                salScript = contact.GetComponent<SalBoss>();
                salScript.takeDamage(damage);
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
        //transform.Translate(velocity * Time.deltaTime);
        rb.velocity = velocity;
        //rb.AddForce(velocity);
        //velocity += ;
    }
}
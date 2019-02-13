using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Projectile
{
    private Vector2 accel;
    public float x_div = -100f;
    public float y_vel = 0.075f;
    public int hits = 4;


    protected override void Start()
    {
        base.Start();
        velocity.y = y_vel;
        accel = new Vector2(velocity.x / x_div, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide(collision);
    }

    protected override void collide(Collider2D collision)
    {
        GameObject contact = collision.gameObject;

        //can't hit player or other projectile
        //if (!contact.tag.Equals("Player") && contact.layer != 15)
        //{
        //Debug.Log("hit something");
        if (contact.layer == 14)
        {
            hits--;
            Debug.Log("hits remaining: " + hits);
            
            Debug.Log("hit!");
            SimpleHostile pscript = contact.GetComponent<SimpleHostile>();
            //Debug.Log(lastDir);
            pscript.takeDamage(damage, dir);
            if (hits <= 0)
                gameObject.SetActive(false);

        }

        if (contact.layer == 8)
            gameObject.SetActive(false);
        //        }


        /*else if (contact.layer.Equals("Bullet"))
    {
        Debug.Log("bullet hit!");


    }*/
    }

    protected override void Update()
    {

        velocity += (Vector3)accel;
        base.Update();
        
        
    }

}

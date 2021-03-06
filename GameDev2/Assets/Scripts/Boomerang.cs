﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Projectile
{
    private Vector2 accel;
    public float div = -100f;
    public float y_vel = 0.075f;
    public int hits = 4;

    float startDir;
    float backDir;
    //Vector2 target_velocity;

    protected override void Start()
    {
        base.Start();
        startDir = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        backDir = startDir + Mathf.PI;
        if (backDir >= 2 * Mathf.PI)
            backDir -= 2 * Mathf.PI;
        //target_velocity = -velocity;
        //velocity.y += y_vel;
        accel = velocity / div;
        //accel.y += y_vel;
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
            SimpleHostile hscript;
            FactoryBoss fscript;
            SalBoss salScript;
            Debug.Log(contact.tag);
            if (contact.tag.Contains("Mantis"))
            {
                fscript = contact.transform.parent.gameObject.GetComponent<FactoryBoss>();
                if (contact.tag.Contains("Head"))
                {
                    //double damage for headshot
                    Debug.Log("Headshot");
                    fscript.takeDamage(2 * damage, dir);
                }
                else if (contact.tag.Equals("Claws"))
                {
                    //regular damage for claws
                    fscript.takeDamage(damage, dir);
                }
                else
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

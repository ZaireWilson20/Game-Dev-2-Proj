using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Projectile
{
    public float accel;
    public int health;

    protected override void Start()
    {
        speed = 1f;
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

        if (contact.layer == 19)
        {
            Debug.Log("Destroy this wall");
            collision.gameObject.SetActive(false);
            health--;
        }

        if (contact.layer == 8)
            health = 0;
        health--;
    }

    protected override void Update()
    {
        velocity += (Vector3) (accel*dir);
        base.Update();
        Debug.Log(velocity);
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

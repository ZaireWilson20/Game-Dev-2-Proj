using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Projectile
{
    public float health;

    protected override void Start()
    {
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
            health--;
        }

        if (contact.layer == 19)
        {
            FreezeState freezeScript;
            SpriteRenderer spr = contact.GetComponent<SpriteRenderer>();
            if (contact.GetComponent<FreezeState>())
                freezeScript = contact.GetComponent<FreezeState>();
            else
            {
                Debug.Log("Thing shouldn't freeze");
                freezeScript = null;
            }
            if (freezeScript != null)
                freezeScript.freeze(true);
            Debug.Log("Should change color here");
        }

        if (contact.layer == 8)
            gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        //damage intensity is reduced as a function of distance from start position
        //when damage has no power, toxic shot disappears
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

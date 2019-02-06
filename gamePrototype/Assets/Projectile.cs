using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    private float lastDir;
    Vector3 velocity = new Vector3(0,0,0);
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //get direction that player is facing

        //Debug.Log("new projectile!");
        velocity.x = speed;
        //transform.Translate(velocity * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject contact = collision.gameObject;

        if (!contact.tag.Equals("Player"))
        {
            Debug.Log("hit something");
            if (contact.layer == 9)
            {
                Debug.Log("hit!");
                SimpleHostile pscript = contact.GetComponent<SimpleHostile>();
                //Debug.Log(lastDir);
                pscript.takeDamage(damage, lastDir);
            }

            gameObject.SetActive(false);
        }


            /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    // Update is called once per frame
    void Update()
    {
        lastDir = Mathf.Sign(rb.velocity.x);
        transform.Translate(velocity * Time.deltaTime);
    }
}

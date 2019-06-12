using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Projectile
{
    public float accel;
    public int health;
    public GameObject gameManagerObj;
    private GameState gameManager;

    protected override void Start()
    {
        speed = 1f;
        base.Start();
        gameManager = gameManagerObj.GetComponent<GameState>();

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

            collide(collision);

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
        if (gameManager.paused)
        {
            Debug.Log("paused");
            rb.velocity = Vector2.zero;
        }
        if (!gameManager.paused) {
            velocity += (Vector3)(accel * dir);
            base.Update();

        }
        this.sprite.flipX = true;
        //Debug.Log(velocity);
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

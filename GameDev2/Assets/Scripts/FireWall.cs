using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : EnemyProjectile
{
    public float trackingTime = 3f;
    private float trackTimer = 0f;
    public float xOffset = 5f;
    public float yOffset = 0f;
    public float timeToDespawn = 2f;
    private float despawnTimer = 0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void collide(Collider2D collision)
    {
        GameObject contact = collision.gameObject;

        if (contact.tag.Equals("Player"))
        {
            Debug.Log("hit!");
            Player pscript = contact.GetComponent<Player>();
            pscript.takeDamage(damage, direction);
        }

        if (contact.layer == 18)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide(collision);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (trackTimer < trackingTime)
        {
            this.transform.position = new Vector2(player.transform.position.x - xOffset, player.transform.position.y - yOffset) ;
            trackTimer += Time.deltaTime;
        }
        else
        {
            if (despawnTimer >= timeToDespawn)
                Destroy(this.gameObject);
            velocity = new Vector2(0, -speed);
            transform.Translate(velocity*Time.deltaTime);
            despawnTimer += Time.deltaTime;
        }
    }
}

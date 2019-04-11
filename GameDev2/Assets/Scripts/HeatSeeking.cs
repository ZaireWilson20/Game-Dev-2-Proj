using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeking : EnemyProjectile
{
    // Start is called before the first frame update
    private bool stopSeeking = false;
    public float accel = .1f;
    protected override void Start()
    {
        speed = 4;
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

        if (contact.layer == 8 || contact.layer == 18)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!stopSeeking)
            direction = Vector3.Normalize(player.transform.position - transform.position);
        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 3f)
            stopSeeking = true;
        speed += accel;
        velocity = direction * speed;
        base.Update();
    }
}

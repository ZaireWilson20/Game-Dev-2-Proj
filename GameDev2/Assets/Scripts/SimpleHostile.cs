﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * One 'bug' is caused when the player jumps over the enemy, causing it
 * to become confused as to which direction it should travel to search, 
 * so the enemy will just stop in place until it returns to start after 3 seconds.
 * 
 */

public class SimpleHostile : MonoBehaviour
{
    //public float detectRadius = 2.0f;
    public float health_max = 2;
    private float health = 2;
    public int attack = 1;
    public float seekSpeed = 1.25f;
    public float attackSpeed = 2f;
    private bool chasing = false;
    private bool searching = false;
    private bool timer_started = true;
    //private bool returning = false;
    public float searchTimeout = 3.0f;
    private float timeleft;
    private float direction = 0f;
    private float freezeTimer = 0f;
    public float freezeDuration = 5f;
    private float lastDir;

    public float detectRadius = 3f;
    public float knockback = 5f;
    private bool facingRight;

    public Vector3 startPos;
    private Vector3 destination;
    private GameObject target;
    private float moveSpeed;
    private Vector2 velocity = new Vector2(0,0);
    public Rigidbody2D rb;
    //public Collider2D detectCollider;
    //public Collider2D hitbox;
    private GameObject player;
    private bool flash = false;
    public int flashRate = 3;
    private int flashCt = 0;
    public bool invincible = false;
    public bool shooter = false;
    public float fireCooldown = 2f;
    public float fireRate = 0.1f;
    private float fireTime = 0f;
    private float nextFire = 0f;
    public float shootRadius = 10f;

   // private Sprite enemSprite; 
    private GameObject newProjectile;
    public GameObject projectile;
    private int shots = 0;
    public int shotSpray = 3;

    private SpriteRenderer sprite;
    private Color origColor;
    //Player pscript;

    private Animator anim;
    private bool dead = false;
    public bool freezable = true;
    private bool frozen = false;

    public bool isFlying = false;
    public LayerMask flyingMask;
    public float flySpeed = 3f;

    [SerializeField]
    //GameObject pa_playerObj;
    Player pa_script;
    // Start is called before the first frame update
    void Start()
    {
        //get starting position
        startPos = transform.position;
        //get rigidbody and colliders
        rb = GetComponent<Rigidbody2D>();
        //detectCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>(); 
        sprite = GetComponent<SpriteRenderer>();
        origColor = sprite.color;
        //get player object
        player = GameObject.FindGameObjectWithTag("Player");
        pa_script = player.GetComponent<Player>();
        //enemSprite = GetComponent<Sprite>();
        health = health_max;
    }


    
    /*
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            //player left detection radius; continue traveling with same velocity
            chasing = false;
            searching = true;
            //start 3 second search timer
            timeleft = searchTimeout;
            moveSpeed = seekSpeed;
            //returning = false;
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject contact = collision.gameObject;
        //Debug.Log("layer: " + gameObject.layer);
        //Debug.Log("grounded: " + pa_script.grounded);
        //if (contact.layer != 8 && contact.layer != 0)
        //    Debug.Log(contact.tag);
        //Debug.Log(collision.gameObject);
        if (contact.tag.Equals("Player"))
        {
            Debug.Log("frozen: " + frozen);
            if (!frozen)
            {
                Debug.Log("I HIT THE PLAYER");
                anim.SetTrigger("Attack");
                //Debug.Log("attack!");
                //Debug.Log(lastDir);
                if (!pa_script.invincible)
                {
                    pa_script.anim.SetTrigger("Hurt");
                    pa_script.takeDamage(attack, rb.velocity);
                }
            }
        }

        //Behavior for flying enemies
        if (isFlying)
        {
            flySpeed *= -1;
            Debug.Log("Switch Dir");
        }
        /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    public void takeDamage(float damage, Vector2 knockDir)
    {
        //Special freeze shot case
        if (damage == 0)
        {

        }
        else if (!invincible)
        {
            health -= damage;
            if (health <= 0)
            {
                //enemy has died
                Debug.Log("Enemy killed!");
                anim.SetBool("Dead", true);
                dead = true; 
                StartCoroutine(deadWait());
                //gameObject.SetActive(false);
            }

            //consider using increasing degree of transparency (via alpha)
            flash = true;
            sprite.enabled = false;
            invincible = true;
            flashCt = 0;
            velocity.x += knockback * knockDir.x;
            velocity.y += knockback * knockDir.y;
            //controller.Move(velocity * Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
            Debug.Log("Enemy health: " + health);
        }
    }

    //checks if enemy has line of sight to player within shoot radius distance
    public bool LineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 dest = player.transform.position;
        Vector2 angle = dest - origin;
        angle.Normalize();
        int layerMask = ~(LayerMask.GetMask("Hostile")) + ~(LayerMask.GetMask("Map")) + ~(LayerMask.GetMask("Reflect"));
        
        RaycastHit2D hit = Physics2D.Raycast(origin, angle, shootRadius, layerMask);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit.point);

        //Debug.Log("Hostile hitting layer: " + hit.collider.gameObject.layer);
        //check if player in line of sight
        
        if (hit.collider == null)
        {
            //Debug.Log("null");
            return false;
        }

        //Debug.Log(hit.collider.gameObject);
        if (hit.collider.tag.Equals("Player"))
        {
            return true;
        }

        return false;
    }

    public void Freeze(bool freezing)
    {
        if (!freezable)
            return;

        if (freezing)
        {
            gameObject.layer = 20;
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            rb.velocity = new Vector2(0, 0);
            sprite.color = new Color(0, 194, 255, 255);
            freezeTimer = freezeDuration;
            frozen = true;
        }
        else
        {
            gameObject.layer = 14;
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            sprite.color = origColor;
            frozen = false;
        }
    }

    void Update()
    {
        //Flying enemies don't take damage and don't follow the player
        if (!isFlying)
        {
            //check if player within detection radius
            if (Vector3.Distance(player.transform.position, transform.position) <= detectRadius)
            {
                target = player;
                //player entered detection radius
                //Debug.Log("attack!");
                //start chasing player
                chasing = true;
                searching = false;
                timer_started = false;
                moveSpeed = attackSpeed;
                //returning = false;
            }
            else
            {
                if (!timer_started)
                {
                    timer_started = true;
                    //player left detection radius; continue traveling with same velocity
                    chasing = false;
                    searching = true;
                    //start 3 second search timer
                    timeleft = searchTimeout;
                    moveSpeed = seekSpeed;
                    //returning = false;
                }
            }

            //enemy blinks once when hit
            if (flash)
            {
                if (flashCt < flashRate)
                {
                    flashCt++;
                }
                else
                {
                    invincible = false;
                    flash = false;
                    sprite.enabled = true;
                }
            }

            //follow player
            if (chasing)
            {
                //track down player
                //Debug.Log("chasing player");
                destination = target.transform.position;
                //GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.transform.position;
            }
            if (!chasing && !searching)
            {
                //return to starting position
                //Debug.Log("returning to start");
                destination = startPos;
                //GetComponent<UnityEngine.AI.NavMeshAgent>().destination = startPos;
            }

            //search for player for 3 seconds before returning to start position
            if (searching && timeleft > 0f)
            {
                timeleft -= Time.deltaTime;     //decrease timer by one update frame
            }
            else if (timeleft <= 0f)
            {
                searching = false;              //stop search and return to start
            }


            //check if enemy is a shooter and can detect player
            fireTime += Time.deltaTime;
            //Debug.Log(fireTime);
            //Debug.Log(nextFire);
            if (!frozen && shooter && fireTime > nextFire)
            {
                if (LineOfSight())
                {
                    //check if shoot timer is approved
                    //fire away!
                    if (shots < shotSpray)
                    {
                        nextFire = fireTime + fireRate;
                        newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                        Debug.Log("Flower Fired");
                        //newProjectile.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(Mathf.Sign(velocity.x),0,0));
                        //newProjectile.velocity = transform.TransformDirection(Vector3.forward * 10);
                        newProjectile.SetActive(true);
                        shots++;
                    }
                    else
                    {
                        shots = 0;
                        nextFire = fireCooldown;
                        fireTime = 0;
                    }
                }
                //else
                //{
                //    shots = 0;
                //}
            }

            //travel towards destination if not within 0.1 of target
            if (!frozen && Mathf.Abs(transform.position.x - destination.x) > 0.1f && !dead)
            {
                //Debug.Log("moving");
                lastDir = direction;
                direction = Mathf.Sign(destination.x - transform.position.x);
                velocity.x = direction * moveSpeed;    //  Speed force added to horizontal velocity, no acceleration
                rb.velocity = velocity;

                if (direction < 0)
                {
                    sprite.flipX = false;
                }
                else if (direction > 0)
                {
                    sprite.flipX = true;
                }

                anim.SetBool("Walking", true);
                //  Damping/acceleration applied throught damping.
                //velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below ? accelTime_ground : accelTime_air);

            }
            //stop moving
            else
            {
                rb.velocity = new Vector2(0, 0);
                anim.SetBool("Walking", false);
            }
        }
        else
        {
            if (flySpeed > 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
            invincible = true;
            velocity.x = flySpeed;
            velocity.y = 0;
            rb.velocity = velocity;
            if (freezeTimer >= 0)
                anim.SetBool("Frozen", true);
            else
                anim.SetBool("Frozen", false);
        }

        //Debug.Log("layer: " + gameObject.layer);
        if (freezeTimer <= 0)
            Freeze(false);
        //Debug.Log(sprite.color);
        freezeTimer -= Time.deltaTime;

   
    }


    IEnumerator deadWait()
    {
        yield return new  WaitForSeconds(.5f);
        gameObject.SetActive(false);

    }
}

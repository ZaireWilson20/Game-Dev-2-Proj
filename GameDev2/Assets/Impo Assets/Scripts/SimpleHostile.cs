using System.Collections;
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

    private GameObject newProjectile;
    public GameObject projectile;
    private int shots = 0;
    public int shotSpray = 3;

    private SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        //get starting position
        startPos = transform.position;
        //get rigidbody and colliders
        rb = GetComponent<Rigidbody2D>();
        //detectCollider = GetComponent<Collider2D>();

        sprite = GetComponent<SpriteRenderer>();
        //get player object
        player = GameObject.FindGameObjectWithTag("Player");

        health = health_max;
    }


    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hello?");
        if (other.tag.Equals("Player"))
        {
            target = other.gameObject;
            //player entered detection radius
            Debug.Log("attack!");
            //start chasing player
            chasing = true;
            searching = false;
            moveSpeed = attackSpeed;
            //returning = false;
        }
    }

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

        if (contact.tag.Equals("Player"))
        {
            //Debug.Log("attack!");
            Player pscript = contact.GetComponent<Player>();
            //Debug.Log(lastDir);
            if (!pscript.invincible)
            {
                pscript.anim.SetTrigger("Hurt");
                pscript.takeDamage(attack, rb.velocity);
            }
        }
        /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    public void takeDamage(float damage, Vector2 knockDir)
    {
        if (!invincible)
        {
            health -= damage;
            if (health <= 0)
            {
                //player has died
                Debug.Log("Enemy killed!");
                gameObject.SetActive(false);
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
        int layerMask = ~(LayerMask.GetMask("Hostile"));
        RaycastHit2D hit = Physics2D.Raycast(origin, angle, shootRadius, layerMask);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit.point);

        //check if player in line of sight
        if (hit.collider == null)
        {
            Debug.Log("null");
            return false;
        }

        Debug.Log(hit.collider.gameObject);
        if (hit.collider.tag.Equals("Player"))
        {
            return true;
        }

        return false;
    }

    void Update()
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
        } else if (timeleft <= 0f)
        {
            searching = false;              //stop search and return to start
        }


        //check if enemy is a shooter and can detect player
        fireTime += Time.deltaTime;
        //Debug.Log(fireTime);
        //Debug.Log(nextFire);
        if (shooter && fireTime > nextFire)
        {
            if (LineOfSight())
            {
                //check if shoot timer is approved
                //fire away!
                if (shots < shotSpray)
                {
                    nextFire = fireTime + fireRate;
                    newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
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
        if (Mathf.Abs(transform.position.x - destination.x) > 0.1f)
        {
            //Debug.Log("moving");
            lastDir = direction;
            direction = Mathf.Sign(destination.x - transform.position.x);
            velocity.x = direction * moveSpeed;    //  Speed force added to horizontal velocity, no acceleration
            rb.velocity = velocity;

            //  Damping/acceleration applied throught damping.
            //velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below ? accelTime_ground : accelTime_air);

        }
        //stop moving
        else
        {
            rb.velocity = new Vector2(0, 0);
        }





    }

}

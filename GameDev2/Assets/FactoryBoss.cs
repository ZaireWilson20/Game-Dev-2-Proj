using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FactoryBoss : MonoBehaviour
{
    //public float detectRadius = 2.0f;
    public float health_max = 20;
    private float health = 20;
    public float shield_health = 5; //at or below this health level, you must headshot the boss...

    //movement variables
    public int mvmtForce = 10;
    Vector2 mvdir = Vector2.left;
    public float dist = 4.5f;
    public float velocityCap = 10f;

    //behavior management
    int behavior = 0;   //start with default
    float time = 0f;
    float specTime = 0f;
    float height;
    GameObject player;
    Player pscript;

    //attack variables
    public float swipeTime = 3f;
    public float swipeDist = 10f;
    public int swipeDamage = 1;

    public float specialAttackTime = 15f;
    public float stompTimer = 5f;
    public bool shielded = false;

    public float collide_attack = 0.5f;
    Collider2D[] colliders;
    //public float seekSpeed = 1.25f;
    //public float attackSpeed = 2f;
    //private bool chasing = false;
    //private bool searching = false;
    //private bool timer_started = true;
    ////private bool returning = false;
    //public float searchTimeout = 3.0f;
    //private float timeleft;
    //private float direction = 0f;
    //private float lastDir;

    //public float detectRadius = 3f;
    public float knockback = 5f;
    //private bool facingRight;

    public Vector3 startPos;
    //private Vector3 destination;
    //private GameObject target;
    //private float moveSpeed;
    private Vector2 velocity = new Vector2(0, 0);
    public Rigidbody2D rb;
    ////public Collider2D detectCollider;
    ////public Collider2D hitbox;
    private bool flash = false;
    public int flashRate = 3;
    private int flashCt = 0;
    public bool invincible = false;
    //public bool shooter = false;
    //public float fireCooldown = 2f;
    //public float fireRate = 0.1f;
    //private float fireTime = 0f;
    //private float nextFire = 0f;
    //public float shootRadius = 10f;

    //// private Sprite enemSprite; 
    //private GameObject newProjectile;
    //public GameObject projectile;
    //private int shots = 0;
    //public int shotSpray = 3;

    private SpriteRenderer sprite;

    //private Animator anim;
    private bool dead = false;

    [SerializeField]
    //GameObject pa_playerObj;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //get starting position
        startPos = transform.position;
        //get rigidbody and colliders
        rb = GetComponent<Rigidbody2D>();
        //detectCollider = GetComponent<Collider2D>();
        //anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //get player object
        player = GameObject.FindGameObjectWithTag("Player");
        pscript = player.GetComponent<Player>();
        //enemSprite = GetComponent<Sprite>();
        health = health_max;
        rb.AddForce(Vector2.left * mvmtForce);
        mvdir = Vector2.left;
        height = sprite.bounds.extents.y;
        anim = GetComponent<Animator>();
        colliders = GetComponentsInChildren<Collider2D>();
        anim.SetInteger("Dead", -1);    //set animator to not dead
    }

    public float GetHealth()
    {
        return health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject contact = collision.gameObject;
        //Debug.Log(collision.gameObject);
        if (contact.tag.Equals("Player") && !dead)
        {
            Debug.Log("I HIT THE PLAYER");
            //anim.SetTrigger("Attack");
            //Debug.Log("attack!");
            Player pscript = contact.GetComponent<Player>();
            //Debug.Log(lastDir);
            if (!pscript.invincible)
            {
                pscript.anim.SetTrigger("Hurt");
                pscript.takeDamage(collide_attack, mvdir);
            }
        }
        /*else if (contact.layer.Equals("Bullet"))
        {
            Debug.Log("bullet hit!");


        }*/
    }

    public void Swipe()
    {
        if (!dead)
        {
            Debug.Log("swipe");
            //play swipe (spit) animation
            bool swipeRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height / 2f), mvdir, swipeDist, LayerMask.GetMask("Player"));
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - height / 2f), mvdir * swipeDist, Color.red);
            if (swipeRay)
            {
                //player hit by swipe
                Debug.Log("swiped the player");
                pscript.takeDamage(swipeDamage, mvdir);
            }
        }
    }

    public void takeDamage(float damage, Vector2 knockDir)
    {
        //anim.SetFloat("Health", health);

        if (!invincible)
        {
            //play hurt animation
            anim.SetTrigger("Damaged");

            health -= damage;
            if (health <= shield_health)
            {
                if (!shielded)
                {
                    //play shielding animation and inform player that legs are shielded now

                }
                shielded = true;
                Debug.Log("shielded");
            }
            if (health <= 0)
            {
                //enemy has died
                //play death animation
                Debug.Log("boss is dying");
                dead = true;
                anim.SetInteger("Dead", 1);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (!dead)
            {
                //turn back to player when damage is taken
                if (transform.position.x - player.transform.position.x > 0)
                    mvdir = Vector2.left;
                else if (transform.position.x - player.transform.position.x < 0)
                    mvdir = Vector2.right;

                flash = true;
                sprite.enabled = false;
                invincible = true;
                flashCt = 0;
                velocity.x += knockback * knockDir.x;
                velocity.y += knockback * knockDir.y;
                //controller.Move(velocity * Time.deltaTime);
                transform.Translate(velocity * Time.deltaTime);
            }
            Debug.Log("Enemy health: " + health);
        }
    }

    private void EnableColliders(bool enabled)
    {
        foreach (Collider2D col in colliders)
            col.enabled = enabled;
    }

    public void Death()
    {
        Debug.Log("Enemy killed!");
        //turn off colliders, disable gravity, and put in background
        GetComponent<Rigidbody2D>().gravityScale = 0;
        EnableColliders(false);
        sprite.sortingLayerName = "Background";
        //gameObject.SetActive(false);
    }

    public void EndSwipe()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (specTime >= specialAttackTime)
        //    behavior = -1;

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

        if (behavior == 0 && !dead)
        {
            //Debug.Log(rb.velocity);
            //move side-to-side using AddForce
            //detect distance from wall
            bool leftRay = Physics2D.Raycast(transform.position, Vector2.left, dist, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, Vector2.left * dist, Color.magenta);
            if (leftRay)
            {
                //boss is within 10 units of left wall
                //Debug.Log("left wall detected");
                //switch to move right
                mvdir = Vector2.right;
                //sprite.flipX = true;
            }
            RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, dist, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, Vector2.right * dist, Color.cyan);
            if (rightRay)
            {
                //boss is within 10 units of right wall
                //Debug.Log("right wall detected");
                //switch to move left
                mvdir = Vector2.left;
                //sprite.flipX = false;
            }
            //make boss face correct direction
            if (mvdir == Vector2.right)
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            else
                transform.rotation = Quaternion.Euler(0, 0, 0);

            rb.AddForce(mvdir * mvmtForce);
            if (rb.velocity.magnitude > velocityCap)
                rb.velocity = mvdir * velocityCap;

            //basic swipe attack
            if (time >= swipeTime)
            {
                //cooldown over; swipe at player
                anim.SetTrigger("Swipe");
                time = 0;
            }
            else
                time += Time.deltaTime;
        }
        //else if (behavior == 1)
        //{
        //    //flight attack
        //    Debug.Log("start flying");


        //} else if (behavior == 2)
        //{
        //    //stomp attack
        //    Debug.Log("Prepare to stomp");


        //} else
        //{
        //    //randomly select special attack
        //    if (Random.value >= 5.0f)
        //    {
        //        behavior = 1;

        //    }
        //    else
        //    {
        //        behavior = 2;
        //        //stompTimer = 0;
        //    }
        //}

        specTime += Time.deltaTime;
    }
}

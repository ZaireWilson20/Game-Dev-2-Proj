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
    public float dist = 10f;
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

    public int attack = 1;
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
    }

    public float GetHealth()
    {
        return health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject contact = collision.gameObject;
        //Debug.Log(collision.gameObject);
        if (contact.tag.Equals("Player"))
        {
            Debug.Log("I HIT THE PLAYER");
            //anim.SetTrigger("Attack");
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

    void Swipe(Vector2 dir)
    {
        Debug.Log("swipe");
        bool swipeRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height / 2), dir, swipeDist, LayerMask.GetMask("Player"));
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - height / 2), dir * swipeDist, Color.red);
        if (swipeRay)
        {
            //player hit by swipe
            Debug.Log("swiped the player");
            pscript.takeDamage(swipeDamage, dir);
        }
    }

    public void takeDamage(float damage, Vector2 knockDir)
    {
        if (!invincible)
        {
            health -= damage;
            if (health <= shield_health)
            {
                shielded = true;
                Debug.Log("shielded");
            }
            if (health <= 0)
            {
                //enemy has died
                Debug.Log("Enemy killed!");
                //anim.SetBool("Dead", true);
                dead = true;
                gameObject.SetActive(false);
            }

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
            Debug.Log("Enemy health: " + health);
        }
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

        if (behavior == 0)
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
            if (mvdir == Vector2.right)
                sprite.flipX = true;
            else
                sprite.flipX = false;
            rb.AddForce(mvdir * mvmtForce);
            if (rb.velocity.magnitude > velocityCap)
                rb.velocity = mvdir * velocityCap;

            //basic swipe attack
            if (time >= swipeTime)
            {
                //cooldown over; swipe at player
                Swipe(mvdir);
                time = 0;
            }
            else
                time += Time.deltaTime;
        }
        else if (behavior == 1)
        {
            //flight attack
            Debug.Log("start flying");


        } else if (behavior == 2)
        {
            //stomp attack
            Debug.Log("Prepare to stomp");


        } else
        {
            //randomly select special attack
            if (Random.value >= 5.0f)
            {
                behavior = 1;

            }
            else
            {
                behavior = 2;
                //stompTimer = 0;
            }
        }

        specTime += Time.deltaTime;
    }
}

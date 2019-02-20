using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CURRENT PLAYER CONTROL LAYOUT
//Non-adjustable
//DirectionalInput movement
//Fire to shoot
//Utility for utility
//Run to run
//Jump to jump/airdash
//H swaps powers

public class Player : MonoBehaviour
{
    //Forces On Player ----------------
    public float gravity;
    public float fast_gravity;
    private float cancel_grav;
    public float speed = 5f;
    public float airdashSpeed = 0.4f;
    public float runSpeed = 10f;

    public float jumpHeight = 8f;
    public float timeToJumpApex = .7f;

    private int flashCt = 0;
    public int flashRate = 5;

    public int health_max = 5;
    private int health = 5;
    public float attack = 1f;
    public float knockback = 5f;
    public float fireDelta = 0.5f;
    public float nextFire = 0.5f;
    public float invincibility = 0.5f;
    public bool invincible = false;
    private float timeLeft = 0.5f;
    private float fireTime = 0.0f;

    SpriteRenderer sprite;
    private GameObject newProjectile;
    public GameObject boomerang;
    public GameObject toxicShot;
    private GameObject projectile;

    public float airdashTime = 0;
    private bool hasAirdash = false;
    private Vector3 airdashDirection = new Vector3(0, 0, 0);

    private bool facingRight = true;

    public bool isSwinging = false;
    private bool wasSwinging = false;
    public Vector2 ropeHook;
    public float swingForce = 4f;

    public float tpDistance = 3f;
    public float tpCooldown = 1f;
    private float timeSinceLastTp;

    //This variable is super important, true means you're in tech mode, false means you're in psychic mode
    public bool powerset = true;

    float jumpVelocity;
    float velocX_smooth;
    float accelTime_air = .4f;
    float accelTime_ground = .1f;
    Vector3 velocity;
    // --------------------------------

    //  Player interactions with environment
    public bool pa_inConvo = false;
    // --------------------------------

    // Animation
    private Rigidbody2D rig2D; 
    private Animator anim;
    private bool idle;
    private bool crouching;
    private bool jumping;
    private bool airDashing;
    //public GameObject playerSprite; 
    // -----------------------------
    private Vector2 directionalInput;

    //  States
    public bool alive = true;
    public bool inGame;

    //  Gmae Manager
    public GameObject gameManagerObj;
    private GameState gameManager; 

    //Calculate airdash direction here
    Vector3 calculateAirdashVector()
    {
        Vector2 vec;
        if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") > 0f)
        {
            vec = new Vector2(airdashSpeed * .65f, airdashSpeed * .65f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") > 0f)
        {
            vec = new Vector2(-airdashSpeed * .65f, airdashSpeed * .65f);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            vec = new Vector2(airdashSpeed, 0f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            vec = new Vector2(-airdashSpeed, 0f);
        }
        else
        {
            vec = new Vector2(0, airdashSpeed*1f);
        }
        return vec;
    }

    //Returns valid directions you can aim in, used for all aimed projectiles, grappling hook
    //All but straight down
    public float aimDirection()
    {
        float aim = 0f;
        if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") > 0f)
        {
            aim = 45f;
            facingRight = true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") > 0f)
        {
            aim = 135f;
            facingRight = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") < 0f)
        {
            aim = -45f;
            facingRight = true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") < 0f)
        {
            aim = -135f;
            facingRight = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            aim = 0f;
            facingRight = true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            aim = 180f;
            facingRight = false;
        }
        else if (Input.GetAxisRaw("Vertical") > 0f)
        {
            aim = 90f;
        }
        if (aim == 0f)
        {
            if (!facingRight)
                aim += 180f;
        }
        return aim;
    }

    //Returns valid directions you can teleport in, includes downward
    public float tpDirection()
    {
        float tp = 0f;
        //Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") > 0f) //Up Right
        {
            tp = 45f;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") > 0f) //Up Left
        {
            tp = 135f;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") < 0f) //Down Right
        {
            tp = -45f;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") < 0f) //Down Left
        {
            tp = -135f;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f) //Left
        {
            tp = 180f;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f) //Right
        {
            tp = 0f;
        }
        else if (Input.GetAxisRaw("Vertical") > 0f) //Up
        {
            tp = 90f;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f) //Down
        {
            tp = -90f;
        }
        else //Neutral
        {
            if (facingRight)
                tp = 0f;
            else
                tp = 180f;
        }
        return tp;
    }
    Controller2D controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rig2D = GetComponent<Rigidbody2D>();
        gameManager = gameManagerObj.GetComponent<GameState>(); 
        //Gravity is directly proportional to given jump height, and disproportional to time it takes to reach maximum jump height
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        fast_gravity = gravity * 2;

        //How high you jump is directly proportional to gravity and the time it takes to reach max jump height
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        timeSinceLastTp = tpCooldown;

        health = health_max;
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.paused)
        {
            //use boomerang if in tech powerset, toxicShot if magic
            if (powerset)
                projectile = boomerang;
            else
                projectile = toxicShot;

            if (controller.cont_collision_info.above || controller.cont_collision_info.below) //  Stops vertical movement if vertical collision detected
            {
                velocity.y = 0;
            }

            if (!pa_inConvo)  // Can move while not in conversation
            {
                directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            }
            else
            {
                directionalInput = new Vector2(0, 0);
            }

            WalkAnim(directionalInput);

            //Flip Player Based on Direction
            if (directionalInput.x > 0)
            {
                sprite.flipX = true;
            }
            else if (directionalInput.x < 0)
            {
                sprite.flipX = false;
            }



            //On the ground, enable grounded only movement here
            if (isSwinging && powerset)
            {
                wasSwinging = true;
                if (directionalInput.x != 0)
                {
                    //1
                    var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                    //2
                    Vector2 perpendicularDirection;
                    if (directionalInput.x < 0)
                    {
                        perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                        var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                        Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                    }
                    else
                    {
                        perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                        var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                        Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                    }

                    var force = perpendicularDirection * swingForce;
                    this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Force);
                }
            }
            else
            {
                //if (GetComponent<DistanceJoint2D>() != null)
                //    GetComponent<DistanceJoint2D>().enabled = false;
                if (controller.cont_collision_info.below)
                {
                    hasAirdash = true;
                    if (Mathf.Abs(fast_gravity) < Mathf.Abs(gravity))
                    {
                        float f = fast_gravity;
                        fast_gravity = gravity;
                        gravity = f;
                    }
                    hasAirdash = true;
                    float temp;
                    //Crouch when down is pressed
                    Transform tf = this.GetComponent<Transform>();
                    /*
                    if (Input.GetKey(KeyCode.S))
                    {
                        //Temp behavior
                        //tf.localScale = new Vector3(5f, 2.5f, 5f);
                        speed = 0;
                        runSpeed = 0;
                        crouching = true;

                    }
                    else
                    {
                        //tf.localScale = new Vector3(5f, 5f, 5f);
                        speed = 5;
                        runSpeed = 10;
                        crouching = false; 
                    }
                    */
                    //Run when holding P
                    if (Input.GetKey(KeyCode.O))
                    {
                        if (speed < runSpeed)
                        {
                            temp = speed;
                            speed = runSpeed;
                            runSpeed = temp;
                        }
                    }
                    else
                    {
                        if (speed > runSpeed)
                        {
                            temp = speed;
                            speed = runSpeed;
                            runSpeed = temp;
                        }
                    }
                    //Jump when space is pressed
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        velocity.y = jumpVelocity;
                        jumping = true;
                    }

                }
                //In the air, enable only air movement here
                else if (!controller.cont_collision_info.below)
                {
                    //Fastfall when down is pressed in the air
                    float temp;
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        temp = gravity;
                        gravity = fast_gravity;
                        fast_gravity = temp;
                    }
                    //Airdash when space is pressed in the air
                    if (hasAirdash && Input.GetKeyDown(KeyCode.Space))
                    {
                        //Used airdash
                        hasAirdash = false;
                        //airdashTime = .3f;
                        //this.airdashDirection = calculateAirdashVector();
                        velocity.y = jumpVelocity * 1.2f;
                    }

                }

                //if (airdashTime-Time.deltaTime > 0)
                //{
                //    airdashTime -= Time.deltaTime;
                //    velocity = airdashDirection;
                //}
                //else if (airdashTime - Time.deltaTime < 0 && airdashTime != 0)
                //{
                //    velocity = new Vector3(0, 0, 0);
                //    airdashTime = 0;
                //}
                //else
                // {
                velocity.y += gravity * Time.deltaTime; //  Gravity constant
                float targetX_velocity = directionalInput.x * speed;    //  Speed force added to horizontal velocity, no acceleration
                                                                        //  Damping/acceleration applied throught damping.
                velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below ? accelTime_ground : accelTime_air);
                //  Call to move function in controller2D class
                //controller.Move(velocity * Time.deltaTime);
                // }
                controller.Move(velocity * Time.deltaTime);

                //TELEPORT LOGIC
                if (timeSinceLastTp > tpCooldown && !powerset)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //Handle teleport
                        float angle = tpDirection();
                        //Debug.Log(angle);
                        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
                        dir.x *= tpDistance;
                        dir.y *= tpDistance;
                        transform.position = transform.position + (Vector3)dir;
                        timeSinceLastTp = 0f;
                    }
                }
                timeSinceLastTp += Time.deltaTime;

                //fire projectile if '1' key pressed and cooldown expired
                fireTime = fireTime + Time.deltaTime;
                if (Input.GetButton("Fire1") && fireTime > nextFire)
                {
                    nextFire = fireTime + fireDelta;
                    newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                    //newProjectile.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(Mathf.Sign(velocity.x),0,0));
                    //newProjectile.velocity = transform.TransformDirection(Vector3.forward * 10);
                    newProjectile.SetActive(true);

                    //check facing of sprite
                    if (sprite.flipX == false)
                    {
                        //sprite facing left (backwards)
                        newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
                    }
                    else
                    {
                        //sprite facing right (forwards)
                        newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
                    }


                    //Debug.Log(newProjectile.GetComponent<Rigidbody2D>().velocity);

                    // create code here that animates the newProjectile
                    //Debug.Log("Fire!");
                    nextFire = nextFire - fireTime;
                    fireTime = 0.0F;
                }

                if (Input.GetKeyDown(KeyCode.R))
                    powerset = !powerset;
            }

            if (invincible)
            {
                //Debug.Log("ignore collision? " + Physics2D.GetIgnoreLayerCollision(9, 11));
                //Debug.Log(invincible);
                if (flashCt % flashRate == 0)
                    sprite.enabled = !sprite.enabled;

                flashCt++;
                timeLeft -= Time.deltaTime;
                //Debug.Log(timeLeft);
                if (timeLeft <= 0.0)
                {
                    invincible = false;
                    Physics2D.IgnoreLayerCollision(13, 14, false);
                    //timer_started = false;
                }
            }
            else
            {
                flashCt = 0;
                sprite.enabled = true;
            }

            if (velocity.x < 0)
            {
                facingRight = true;
            }
            else if (velocity.x > 0)
            {
                facingRight = false;
            }
            JumpAnim();
            CrouchAnim();
            AirDashAnim();
        }
    }

    public void takeDamage(int damage, float knockDir)
    {
        Debug.Log("invincible: " + invincible);
        if (!invincible)
        {
            health -= damage;
            if (health == 0)
            {
                //player has died
                Debug.Log("Player died!");
                gameObject.SetActive(false);
            }
            velocity.x += knockback * knockDir;
            controller.Move(velocity * Time.deltaTime);
            Debug.Log("Player health: " + health);
            //timer_started = true;
            timeLeft = invincibility;
            //turn off collision with enemies for 0.5 seconds
            invincible = true;
            Debug.Log("invincible: true");
            Physics2D.IgnoreLayerCollision(13, 14, true);
        }

        //Animation Update; 
        WalkAnim(directionalInput);
        CrouchAnim();
        JumpAnim();
    }

    void WalkAnim(Vector2 input)
    {
        if(Mathf.Abs(velocity.x) > .005 && input.x != 0)
        {
            anim.SetBool("Walk", true);
            idle = false; 
        }
        else
        {
            if (!idle)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
                idle = true; 
            }
        }
    }

    void CrouchAnim()
    {
        if (crouching)
        {
            anim.SetBool("Crouch", true);
        }
        else
        {
            anim.SetBool("Crouch", false);
        }
    }

    void JumpAnim()
    {

        if (jumping && !controller.cont_collision_info.below)
        {
            if (jumping)
            {
                anim.SetBool("Grounded", false);
                anim.SetBool("Jump_Ascend", true);
                anim.SetBool("Walk", false);
            }
        }
        else
        {
            jumping = false;
            anim.SetBool("Grounded", true);
            anim.SetBool("Jump_Ascend", false);
        }

    }

    void AirDashAnim()
    {
        //anim.SetBool("Air Dash", airDashing);
        //airDashing = false; 
    }
}

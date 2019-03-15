using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

//CURRENT PLAYER CONTROL LAYOUT
//Non-adjustable
//DirectionalInput movement
//Fire to shoot
//Utility for utility
//Run to run
//Jump to jump/airdash
//H+Q swaps powers

//name = name of the power
//active = if you have access to the power
//side = which powerset it belongs to
public class Power : IComparable<Power>
{
    public string name;
    public bool active;
    public bool side;
    public Power(string n, bool a, bool s)
    {
        name = n;
        active = a;
        side = s;
    }

    public int CompareTo(Power other)
    {
        return name.CompareTo(other.name);
    }

    public void toString()
    {
        Debug.Log(name + ", " + active + ", " + side);
    }

    public void setActive()
    {
        active = true;
    }
}

public class Player : MonoBehaviour
{
    //saved player data
    public PlayerData localPlayerData = new PlayerData();

    //Forces On Player ----------------
    public float speed = 5f;
    public float airdashSpeed = 20f;
    public float runSpeed = 10f;
    public float jumpHeight = 20f;
    public bool grounded = true;
    private bool fastFall = false;
    public float fastFallSpeed = 6f;
    public float fallSpeed = 3f;
    public float swingGrav = 1f;
    public float groundedDist = .5f; 

    private int flashCt = 0;
    public int flashRate = 5;

    public bool alive = true;
    public int health_max = 5;
    private int health = 5; //save
    public float attack = 1f;
    public float knockback = 5f;
    public float fireDelta = 0.5f;
    public float nextFire = 0.5f;
    public float invincibility = 0.5f;
    public bool invincible = false;
    private float timeLeft = 0.5f;
    private float fireTime = 1f;
    public float shootDelay = .02f;

    SpriteRenderer sprite;
    private GameObject newProjectile;
    public GameObject boomerangObj;
    public GameObject toxicShot;
    private GameObject projectile;

    public float airdashTime = 0;
    public bool hasAirdash = false;
    private Vector3 airdashDirection = new Vector3(0, 0, 0);

    private bool facingRight = true;

    public bool isSwinging = false;
    private bool wasSwinging = false;
    public Vector2 ropeHook;
    public float swingForce = 4f;

    public float tpDistance = 3f;
    public float tpCooldown = 1f;
    private float timeSinceLastTp;

    public LayerMask floorMask;

    //This variable is super important, true means you're in tech mode, false means you're in psychic mode
    public bool powerset = true;
    public Dictionary<string, Power> tPowerDict = new Dictionary<string, Power>();
    public Dictionary<string, Power> tWeaponDict = new Dictionary<string, Power>();
    public Dictionary<string, Power> mPowerDict = new Dictionary<string, Power>();
    public Dictionary<string, Power> mWeaponDict = new Dictionary<string, Power>();
    public Power tWeapon;
    public Power tUtility;
    public Power mWeapon;
    public Power mUtility;
    private Power boomerang;
    private Power poisonShot;
    private Power grapple;
    private Power teleport;
    private Power antiGrav;
    private Power reflector;
    private Power drill;
    private Power freeze;

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
    public Animator anim;
    private bool idle;
    private bool crouching;
    private bool jumping;
    private bool airDashing;
    public bool doneShooting;
    private Vector2 directionalInput;
    private float halfHeight;

    //  Game Manager
    public GameObject gameManagerObj;
    private GameState gameManager;
    public string levelName;

    //  UI
    public GameObject healthObj;
    private HealthUI hiScript;
    public GameObject pSetObj;
    private PowerSetController pSetCont;
    


    //Calculate airdash direction here
    Vector3 calculateAirdashVector()
    {
        Vector2 vec;
        if (Input.GetAxis("Horizontal") > 0f && Input.GetAxis("Vertical") > .25f && Input.GetAxis("Vertical") < .75f)
        {
            vec = new Vector2(airdashSpeed * .65f, airdashSpeed * .85f);
        }
        else if (Input.GetAxis("Horizontal") < 0f && Input.GetAxis("Vertical") > .25f && Input.GetAxis("Vertical") < .75f)
        {
            vec = new Vector2(-airdashSpeed * .65f, airdashSpeed * .85f);
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
            vec = new Vector2(0, airdashSpeed * 1f);
        }
        Debug.Log(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        return vec;
    }

    //Returns valid directions you can aim in, used for all aimed projectiles
    public float aimDirection()
    {
        float aim = 0f;
        var inputDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if (inputDir.x == 0f && inputDir.y == 0f)
        {
            if (facingRight)
                inputDir.x = 1f;
            else
                inputDir.x = -1f;
        }
        if (Mathf.Abs(Input.GetAxis("RStickV")) > .05f)
            inputDir.y = Input.GetAxis("RStickV");
        aim = Mathf.Atan2(inputDir.y, inputDir.x);
        return aim*Mathf.Rad2Deg;
    }

    //Returns valid directions you can teleport in, includes downward
    public float tpDirection()
    {
        float tp = 0f;
        //Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") > 0f) //Up Right
        //{
        //    tp = 45f;
        //}
        //else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") > 0f) //Up Left
        //{
        //    tp = 135f;
        //}
        //else if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") < 0f) //Down Right
        //{
        //    tp = -45f;
        //}
        //else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") < 0f) //Down Left
        //{
        //    tp = -135f;
        //}
        if (Input.GetAxisRaw("Horizontal") < 0f) //Left
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
        //Debug.Log("start");
        //load player save data
        //Debug.Log("global" + GlobalControl.Instance.savedPlayer.playerHealth);

        localPlayerData = GlobalControl.Instance.savedPlayer;
        health = localPlayerData.playerHealth;
        //Debug.Log(health);

        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rig2D = GetComponent<Rigidbody2D>();
        gameManager = gameManagerObj.GetComponent<GameState>();

        timeSinceLastTp = tpCooldown;
        halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        //health = health_max;

        hiScript = healthObj.GetComponent<HealthUI>();
        //health = health_max;
        pSetCont = pSetObj.GetComponent<PowerSetController>();

        //Initialize powers
        boomerang = new Power("boomerang", true, true);
        grapple = new Power("grapple", true, true);
        poisonShot = new Power("poison", true, false);
        teleport = new Power("teleport", true, false);
        antiGrav = new Power("anti-grav", false, true);
        drill = new Power("drill", false, true);
        freeze = new Power("freeze", false, false);
        reflector = new Power("reflect", false, false);
        tPowerDict.Add("grapple", grapple); tPowerDict.Add("anti-grav", antiGrav);
        tWeaponDict.Add("boomerang", boomerang); tWeaponDict.Add("drill", drill);
        mPowerDict.Add("teleport", teleport); mPowerDict.Add("reflector", reflector);
        mWeaponDict.Add("poison", poisonShot); mWeaponDict.Add("freeze", freeze);
        tUtility = grapple;
        tWeapon = boomerang;
        mUtility = teleport;
        mWeapon = poisonShot;
        tPowerDict[grapple.name].toString();
        Physics2D.IgnoreLayerCollision(13, 14, false);

        //Debug.Log(tWeaponDict.ToString());
        //Debug.Log(mPowerDict.ToString());
        //Debug.Log(mWeaponDict.ToString());
    }

    public void SavePlayer()
    {
        localPlayerData.playerHealth = health;
        GlobalControl.Instance.savedPlayer = localPlayerData;
        //Debug.Log("global" + GlobalControl.Instance.savedPlayer.playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //use boomerang if in tech powerset, toxicShot if magic
        if (powerset)
        {
            projectile = boomerangObj;
            pSetCont.showPowerSet("SCIENCE");
            anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Sprites/GameObject.controller", typeof(RuntimeAnimatorController));
        }
        else
        {
            projectile = toxicShot;
            pSetCont.showPowerSet("MAGIC");
            anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Sprites/ParacelsysMAGIC/updatedFullController.controller", typeof(RuntimeAnimatorController));
        }

        if (controller.cont_collision_info.above || grounded) //  Stops vertical movement if vertical collision detected
        {
            velocity.y = 0;
        }

        if (!pa_inConvo)  // Can move while not in conversation
        {
            directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

            //While the player is swinging, limit their abilities to just grapple control
            if (isSwinging && powerset)
            {
                rig2D.gravityScale = swingGrav;
                wasSwinging = true;
                airdashTime = 0f;
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
                if (Input.GetButtonDown("Swap"))
                {
                    if (powerset)
                        tUtility = CyclePower(tPowerDict, tUtility);
                    else
                        mUtility = CyclePower(mPowerDict, mUtility);
                }
                if (Input.GetButtonDown("Utility") && tUtility.name == "anti-grav" && powerset)
                {
                    fallSpeed *= -1;
                    fastFallSpeed *= -1;
                    swingGrav *= -1;
                    jumpHeight *= -1;
                    rig2D.gravityScale = fallSpeed;
                    sprite.flipY = !sprite.flipY;
                }
                //While on the ground, enable all grounded options
                if (grounded)
                {
                    hasAirdash = true;
                    fastFall = false;
                    GetComponent<Rigidbody2D>().gravityScale = fallSpeed;
                    hasAirdash = true;
                    float temp;
                    //Crouch when down is pressed
                    Transform tf = this.GetComponent<Transform>();
                    //if (Input.GetKey(KeyCode.S))
                    //{
                    //    //Temp behavior
                    //    tf.localScale = new Vector3(5f, 2.5f, 5f);
                    //    speed = 0;
                    //    runSpeed = 0;
                    //}
                    //else
                    //{
                    //    tf.localScale = new Vector3(5f, 5f, 5f);
                    //    speed = 5;
                    //    runSpeed = 10;
                    //}
                    //Run when holding P
                    if (Input.GetButton("Run"))
                    {
                        anim.SetBool("Running", true);
                        if (speed < runSpeed)
                        {
                            temp = speed;
                            speed = runSpeed;
                            runSpeed = temp;
                        }
                    }
                    else
                    {
                        anim.SetBool("Running", false);
                        if (speed > runSpeed)
                        {
                            temp = speed;
                            speed = runSpeed;
                            runSpeed = temp;
                        }
                    }
                    //Jump when space is pressed
                    if (Input.GetButtonDown("Jump"))
                    {
                        rig2D.velocity = new Vector2(rig2D.velocity.x, jumpHeight);
                        jumping = true;
                    }
                    else if (Input.GetButtonUp("Jump"))
                    {
                        anim.ResetTrigger("Jump");
                    }
                }
                //In the air, enable only air movement here
                else if (!grounded)
                {
                    //Fastfall when down is pressed in the air
                    if (Input.GetAxisRaw("Vertical") < 0f)
                    {
                        GetComponent<Rigidbody2D>().gravityScale = fastFallSpeed;
                        fastFall = true;
                    }
                    //Airdash when space is pressed in the air
                    if (hasAirdash && Input.GetButtonDown("Jump") && !wasSwinging)
                    {
                        //Used airdash
                        hasAirdash = false;
                        airdashTime = .3f;
                        this.airdashDirection = calculateAirdashVector();
                        anim.SetTrigger("AirDash");
                    }
                }

                if (airdashTime - Time.deltaTime > 0)
                {
                    airdashTime -= Time.deltaTime;
                    rig2D.velocity = airdashDirection;
                }
                else if (airdashTime - Time.deltaTime < 0 && airdashTime != 0)
                {
                    rig2D.velocity = new Vector2(0, 0);
                    velocity.y = 0;
                    velocity.x = 0;
                    airdashTime = 0;
                }
                else
                {
                    float targetX_velocity = directionalInput.x * speed;    //  Speed force added to horizontal velocity, no acceleration
                                                                            //  Damping/acceleration applied throught damping.
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, grounded ? accelTime_ground : accelTime_air);
                    //  Call to move function in controller2D class
                    controller.Move(velocity * Time.deltaTime);
                }

                //TELEPORT LOGIC
                if (timeSinceLastTp > tpCooldown && !powerset)
                {
                    if (Input.GetButton("Utility") || Input.GetKeyDown(KeyCode.E))
                    {
                        //Handle teleport
                        float angle = tpDirection();
                        //Debug.Log(angle);
                        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
                        dir.x *= tpDistance;
                        dir.y *= tpDistance;
                        transform.position = transform.position + (Vector3)dir;
                        timeSinceLastTp = 0f;
                        anim.SetTrigger("Tele");
                    }
                }
                timeSinceLastTp += Time.deltaTime;

                //fire projectile if '1' key pressed and cooldown expired
                fireTime = fireTime + Time.deltaTime;
                if (Input.GetButton("Fire") && fireTime > nextFire)
                {
                    nextFire = fireTime + fireDelta;
                    //newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                    //newProjectile.SetActive(true);

                    ////check facing of sprite
                    //if (sprite.flipY)
                    //{
                    //    //sprite facing left (backwards)
                    //    newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
                    //}
                    //else
                    //{
                    //    //sprite facing right (forwards)
                    //    newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
                    //}


                    //Debug.Log(newProjectile.GetComponent<Rigidbody2D>().velocity);

                    // create code here that animates the newProjectile
                    //Debug.Log("Fire!");
                    if (!powerset)
                    {
                        ToxicShotAnim();
                    }
                    else
                    {
                        BoomerangShotAnim();
                    }
                    doneShooting = false;
                    anim.SetBool("DoneShooting", doneShooting);
                    nextFire = nextFire - fireTime;
                    fireTime = 0.0F;
                    StartCoroutine(ShootAfterTime(shootDelay));
                }

                if (Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.Q))
                    powerset = !powerset;
                wasSwinging = false;
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
                }
            }
            else
            {
                flashCt = 0;
                sprite.enabled = true;
            }

            if (velocity.x > 0)
            {
                facingRight = true;
            }
            else if (velocity.x < 0)
            {
                facingRight = false;
            }
            //Ray blah = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight - .2f), Vector2.down, 0.025f, floorMask);
            if (fallSpeed > 0)
                grounded = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, groundedDist, floorMask);
            else
                grounded = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, groundedDist, floorMask);

            //Debug.DrawRay(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, Color.magenta);

            anim.SetFloat("Falling", rig2D.velocity.y);
            anim.SetBool("Grounded", grounded);
            if (jumping)
            {
                JumpAnim();
            }
            //grounded = controller.cont_collision_info.below;
            if (isSwinging)
            {
                anim.SetTrigger("Grapple");
                anim.SetBool("Swinging", true);
            }
            else
            {
                anim.SetBool("Swinging", false);
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                tPowerDict[antiGrav.name].active = !tPowerDict[antiGrav.name].active;
                tPowerDict[antiGrav.name].toString();
            }
            tUtility.toString();
            tWeapon.toString();
            mUtility.toString();
            mWeapon.toString();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 14)
            Debug.Log("I HIT THE PLAYER");

    }

    //Param 1 - The dictionary of available powers to switch to
    //Param 2 - The current power
    public Power CyclePower(Dictionary<string, Power> dict, Power curr)
    {
        bool takeNext = false;
        bool f = true;
        Power first = null;
        foreach(Power p in dict.Values)
        {
            if (f)
            {
                first = p;
                f = false;
            }
            if (takeNext && p.active)
            {
                p.toString();
                return dict[p.name];
            }
            if (p.name == curr.name)
            {
                takeNext = true;
            }
        }
        first.toString();
        return first;
    }

    public bool ActivatePower(Power NewPower)
    {
        return NewPower.active = true;
    }

    public void takeDamage(int damage, Vector2 knockDir)
    {
        
        //Debug.Log("invincible: " + invincible);
        if (!invincible)
        {
            hiScript.loseHealth();
            health -= damage;
            if (health <= 0)
            {
                //player has died
                Debug.Log("Player died!");
                //health = health_max;
                GlobalControl.Instance.savedPlayer.playerHealth = health_max;
                SceneManager.LoadScene(levelName, LoadSceneMode.Single);
                //gameObject.SetActive(false);
            }
            velocity.x += knockback * knockDir.x;
            velocity.y += knockback * knockDir.y;
            controller.Move(velocity * Time.deltaTime);
            Debug.Log("Player health: " + health);
            timeLeft = invincibility;
            //turn off collision with enemies for 0.5 seconds
            invincible = true;
            //Debug.Log("invincible: true");
            Physics2D.IgnoreLayerCollision(13, 14, true);
        }

    }

    void WalkAnim(Vector2 input)
    {
        if (input.x != 0)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Idle", false);
            idle = false;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
            idle = true;
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
        //Debug.Log("Jumping: " + jumping + " Grounded: " + grounded);
        /*if (jumping)
        {
            jumping = false;
            anim.SetBool("Grounded", false);
            anim.SetBool("Jump_Ascend", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", false);
        }
        else if(grounded)
        {
            anim.SetBool("Jump_Ascend", false);
            anim.SetBool("Grounded", true);
            //anim.SetBool("Jump_Ascend", false);
        }*/
        anim.SetTrigger("Jump");
        jumping = false;
    }

    void BoomerangShotAnim()
    {
        anim.SetTrigger("BoomShot");
      
    }

    void ToxicShotAnim()
    {
        anim.SetTrigger("ToxShot");
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
    }

    IEnumerator ShootAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        newProjectile.SetActive(true);
        //if (sprite.flipX == false)
        //{
        //    //sprite facing left (backwards)
        //    newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
        //}
        //else
        //{
        //    //sprite facing right (forwards)
        //    newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
        //}
        Vector2 shootDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * aimDirection()), Mathf.Sin(Mathf.Deg2Rad * aimDirection()));
        newProjectile.GetComponent<Rigidbody2D>().velocity = shootDir;
        doneShooting = true;
        anim.SetBool("BoomShot", false);
        // Code to execute after the delay
    }


}

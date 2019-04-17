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
    public bool inList = false; 
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
    //saved player/scene data
    public PlayerData localPlayerData = new PlayerData();
    private SceneData localScene = new SceneData();


    //Forces On Player ----------------
    public float speed = 5f;
    public float airdashSpeed = 20f;
    public float runSpeed = 10f;
    public float terminalVel = 20f;
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
    private int health_max;
    private float health = 5; //save
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
    public GameObject drillObj;
    public GameObject freezeObj;
    private GameObject projectile;
    public GameObject reflectWall;

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

    public float reflectCooldown = 4f;
    private float refMax;

    private RopeSystem rs;
    public float AGTimer;
    private float AGTimeLeft;

    private int powerBoost = 0;
    public int points;
    public bool canSwitch = false;

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
    public Vector3 velocity;
    private Vector2 storeVel;
    private bool justPaused = false;
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
    public Vector3 spawnPosition;

    //  UI
    public GameObject healthObj;
    private HealthUI hiScript;
    public GameObject pSetObj;
    private PowerSetController pSetCont;
    private GainedUpgrade powerNotif;

    //Calculate airdash direction here
    Vector3 calculateAirdashVector()
    {
        Vector2 vec;
        if (Input.GetAxis("Horizontal") > .25f && Input.GetAxis("Vertical") > .25f && Input.GetAxis("Vertical") <= .75f)
        {
            vec = new Vector2(airdashSpeed * .65f, airdashSpeed * .85f);
        }
        else if (Input.GetAxis("Horizontal") < -.25f && Input.GetAxis("Vertical") > .25f && Input.GetAxis("Vertical") <= .75f)
        {
            vec = new Vector2(-airdashSpeed * .65f, airdashSpeed * .85f);
        }
        else if (Input.GetAxis("Horizontal") > .25f && Input.GetAxis("Vertical") > -.25f && Input.GetAxis("Vertical") <= .25f)
        {
            vec = new Vector2(airdashSpeed, 0f);
        }
        else if (Input.GetAxis("Horizontal") < -.25f && Input.GetAxis("Vertical") > -.25f && Input.GetAxis("Vertical") <= .25f)
        {
            vec = new Vector2(-airdashSpeed, 0f);
        }
        else if (Input.GetAxis("Horizontal") < -.25f && Input.GetAxis("Vertical") <= -.25f && Input.GetAxis("Vertical") > -.75f)
        {
            vec = new Vector2(-airdashSpeed * .65f, -airdashSpeed * .85f);
        }
        else if (Input.GetAxis("Horizontal") > .25f && Input.GetAxis("Vertical") <= -.25f && Input.GetAxis("Vertical") > -.75f)
        {
            vec = new Vector2(airdashSpeed * .65f, -airdashSpeed * .85f);
        }
        else if (Input.GetAxis("Horizontal") >= -.25f && Input.GetAxis("Horizontal") <= .25f && Input.GetAxis("Vertical") < -.25f)
        {
            vec = new Vector2(0, -airdashSpeed);
        }
        else if (Input.GetAxis("Horizontal") >= -.25f && Input.GetAxis("Horizontal") <= .25f && Input.GetAxis("Vertical") > .25f)
        {
            vec = new Vector2(0, airdashSpeed);
        }
        else
        {
            if (fallSpeed > 0)
                vec = new Vector2(0, airdashSpeed * 1f);
            else
                vec = new Vector2(0, airdashSpeed * -1f);
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
        if (GlobalControl.Instance.savedScene.inCutScene)
        {
            gameObject.SetActive(false);
        }
        localPlayerData = GlobalControl.Instance.savedPlayer;
        health = localPlayerData.playerHealth;
        health_max = localPlayerData.playerHealthCap;
        points = localPlayerData.points;
        canSwitch = localPlayerData.canSwitch;
        spawnPosition = localPlayerData.spawnPosition;
        //Debug.Log(health);

        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rig2D = GetComponent<Rigidbody2D>();
        gameManager = gameManagerObj.GetComponent<GameState>();

        timeSinceLastTp = tpCooldown;
        refMax = 0f;
        halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        //health = health_max;

        //UI Inits
        hiScript = healthObj.GetComponent<HealthUI>();
        //health = health_max;
        pSetCont = pSetObj.GetComponent<PowerSetController>();

        boomerang = new Power("boomerang", true, true);
        grapple = new Power("grapple", true, true);
        poisonShot = new Power("poison", true, false);
        teleport = new Power("teleport", true, false);
        antiGrav = new Power("anti-grav", false, true);
        drill = new Power("drill", false, true);
        freeze = new Power("freeze", false, false);
        reflector = new Power("reflect", false, false);

        //Initialize powers
        if (!localPlayerData.reload)
        {
            tPowerDict.Add("grapple", grapple); tPowerDict.Add("anti-grav", antiGrav);
            tWeaponDict.Add("boomerang", boomerang); tWeaponDict.Add("drill", drill);
            mPowerDict.Add("teleport", teleport); mPowerDict.Add("reflect", reflector);
            mWeaponDict.Add("poison", poisonShot); mWeaponDict.Add("freeze", freeze);
            tUtility = grapple;
            tWeapon = boomerang;
            mUtility = teleport;
            mWeapon = poisonShot;
        }
        //tPowerDict[grapple.name].toString();
        Physics2D.IgnoreLayerCollision(13, 14, false);

        //Debug.Log(tWeaponDict.ToString());
        //Debug.Log(mPowerDict.ToString());
        //Debug.Log(mWeaponDict.ToString());
        
        if (localPlayerData.reload)
        {
            tWeapon = GlobalControl.Instance.savedPlayer.tWeap;
            tUtility = GlobalControl.Instance.savedPlayer.tUtil;
            mWeapon = GlobalControl.Instance.savedPlayer.mWeap;
            mUtility = GlobalControl.Instance.savedPlayer.mUtil;
            tWeaponDict = GlobalControl.Instance.savedPlayer.tWeaps;
            tPowerDict = GlobalControl.Instance.savedPlayer.tUtils;
            mWeaponDict = GlobalControl.Instance.savedPlayer.mWeaps;
            mPowerDict = GlobalControl.Instance.savedPlayer.mUtils;
        }

        pSetCont.SetMPowerImg(mUtility.name);
        pSetCont.SetMWeaponImg(mWeapon.name);
        pSetCont.SetSPowerImg(tUtility.name);
        pSetCont.SetSWeaponImg(tWeapon.name);

        rs = this.GetComponent<RopeSystem>();
        if (spawnPosition != null)
            transform.position = spawnPosition;

        powerNotif = FindObjectOfType<GainedUpgrade>();
    }

    public void SavePlayer()
    {
        localPlayerData.playerHealth = health;
        localPlayerData.playerHealthCap = health_max;
        localPlayerData.points = points;
        localPlayerData.canSwitch = canSwitch;
        localPlayerData.spawnPosition = spawnPosition;

        localPlayerData.mUtil = mUtility;       localPlayerData.tUtil = tUtility;
        localPlayerData.mWeap = mWeapon;        localPlayerData.tWeap = tWeapon;

        localPlayerData.tUtils = tPowerDict;    localPlayerData.mUtils = mPowerDict;
        localPlayerData.tWeaps = tWeaponDict;   localPlayerData.mWeaps = mWeaponDict;

        localPlayerData.reload = true;

        GlobalControl.Instance.savedPlayer = localPlayerData;
        //Debug.Log("global" + GlobalControl.Instance.savedPlayer.playerHealth);

    }

    public void SaveSceneData()
    {
        //save important scene details before leaving
        localScene.lastScene = SceneManager.GetActiveScene().name;

        GlobalControl.Instance.savedScene = localScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalControl.Instance.savedScene.inCutScene){gameObject.SetActive(true);}
        if (Input.GetKeyDown(KeyCode.Alpha1))   //  Lose health for debugging purposes. Should Be Deleted at some point
        {
            health--;
            hiScript.loseHealth(health);
        }
        //use boomerang if in tech powerset, toxicShot if magic
        if (powerset)
        {
            if (tWeapon.name == boomerang.name)
                projectile = boomerangObj;
            else if (tWeapon.name == drill.name)
                projectile = drillObj;
            pSetCont.ShowTSet();
            //pSetCont.showPowerSet("SCIENCE");
#if UNITY_EDITOR
            anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Sprites/ParacelsysMAGIC/tempController/ParaScience.controller", typeof(RuntimeAnimatorController));
#else
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("ParaScience");
#endif
        }
        else
        {
            if (mWeapon.name == poisonShot.name)
                projectile = toxicShot;
            else if (mWeapon.name == freeze.name)
                projectile = freezeObj;
            pSetCont.ShowMSet();
            //pSetCont.showPowerSet("MAGIC");
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Sprites/ParacelsysMAGIC/updatedFullController.controller", typeof(RuntimeAnimatorController));
#else
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("updatedFullController");
#endif

        }

        if (controller.cont_collision_info.above || grounded) //  Stops vertical movement if vertical collision detected
        {
            velocity.y = 0;
        }
        if (gameManager.paused)
        {
            //this.GetComponent<Rigidbody2D>().isKinematic = true;
            //this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            rig2D.gravityScale = 0;
            storeVel = rig2D.velocity;
            rig2D.velocity = Vector2.zero;
            justPaused = true;
            anim.enabled = false;
        } else if (!gameManager.paused) {
            if (justPaused)
            {
                justPaused = false;
                rig2D.gravityScale = fallSpeed;
                rig2D.velocity = storeVel;
                anim.enabled = true;
            }
            if (!pa_inConvo && !gameManager.cutscene)  // Can move while not in conversation
            {
                //grounded = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, groundedDist, floorMask);
                //Debug.DrawRay(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, Color.magenta);
                if (fallSpeed > 0)
                {
                    grounded = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, groundedDist, floorMask) ||
                               Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x - .3f, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, groundedDist, floorMask) ||
                               Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x + .4f, sprite.transform.localPosition.y - halfHeight / 2), Vector2.down, groundedDist, floorMask);


                }
                else
                {
                    grounded = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, groundedDist, floorMask) ||
                               Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x - .3f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, groundedDist, floorMask) ||
                               Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x + .4f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, groundedDist, floorMask);
                }
                Debug.DrawRay(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y + halfHeight / 2), Vector2.down, Color.green);
                Debug.DrawRay(new Vector2(sprite.transform.localPosition.x - .4f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.down, Color.green);
                Debug.DrawRay(new Vector2(sprite.transform.localPosition.x + .4f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.down, Color.green);
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
                        {
                            tUtility = CyclePower(tPowerDict, tUtility);
                            pSetCont.SetSPowerImg(tUtility.name);
                        }
                        else
                        {
                            mUtility = CyclePower(mPowerDict, mUtility);
                            pSetCont.SetMPowerImg(mUtility.name);
                        }
                    }
                    if (Input.GetButtonDown("SwitchWep"))
                    {
                        if (powerset)
                        {
                            tWeapon = CyclePower(tWeaponDict, tWeapon);
                            pSetCont.SetSWeaponImg(tWeapon.name);
                        }
                        else
                        {
                            mWeapon = CyclePower(mWeaponDict, mWeapon);
                            Debug.Log(mWeapon.name);
                            pSetCont.SetMWeaponImg(mWeapon.name);
                        }
                    }
                    //ANTI-GRAVITY LOGIC
                    if ((Input.GetButtonDown("Utility") || Input.GetKeyDown(KeyCode.Alpha3)) && tUtility.name == "anti-grav" && powerset)
                    {
                        if (AGTimeLeft <= 0)
                        {
                            Debug.Log("Anti grav activated");
                            fallSpeed *= -1;
                            fastFallSpeed *= -1;
                            swingGrav *= -1;
                            jumpHeight *= -1;
                            rig2D.gravityScale = fallSpeed;
                            sprite.flipY = !sprite.flipY;
                            AGTimeLeft = AGTimer;
                        }
                    }
                    AGTimeLeft -= Time.deltaTime;
                    //REFLECT WALL LOGIC
                    if (Input.GetButtonDown("Utility") && mUtility.name == "reflect" && !powerset)
                    {
                        if (refMax <= 0)
                        {
                            refMax = reflectCooldown;
                            GameObject newWall = Instantiate(reflectWall, transform.position, transform.rotation) as GameObject;
                            if (facingRight)
                                newWall.transform.position = new Vector2(newWall.transform.position.x + 3, newWall.transform.position.y);
                            else
                                newWall.transform.position = new Vector2(newWall.transform.position.x - 3, newWall.transform.position.y);
                            newWall.SetActive(true);
                        }
                    }
                    refMax -= Time.deltaTime;
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
                            Debug.Log("I jumped");
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
                        if ((Input.GetAxisRaw("Vertical") < 0f && fallSpeed > 0) || (Input.GetAxisRaw("Vertical") > 0f && fallSpeed < 0))
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
                            Debug.Log("Air Dashing");
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
                        if (velocity.x > terminalVel) velocity.x = terminalVel;
                        if (velocity.y > terminalVel) velocity.y = terminalVel;
                        controller.Move(velocity * Time.deltaTime);
                    }

                    //TELEPORT LOGIC
                    if (timeSinceLastTp > tpCooldown && !powerset)
                    {
                        if (Input.GetButtonDown("Utility") && mUtility.name == "teleport")
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
                    if (Input.GetButtonDown("Change Side") && canSwitch)
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
            }
            //Ray blah = Physics2D.Raycast(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y - halfHeight - .2f), Vector2.down, 0.025f, floorMask);

            //Debug.DrawRay(new Vector2(sprite.transform.localPosition.x, sprite.transform.localPosition.y + halfHeight / 2), Vector2.down, Color.magenta);
            //Debug.DrawRay(new Vector2(sprite.transform.localPosition.x-.4f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, Color.magenta);
            //Debug.DrawRay(new Vector2(sprite.transform.localPosition.x+.4f, sprite.transform.localPosition.y + halfHeight / 2), Vector2.up, Color.magenta);

            anim.SetFloat("Falling", rig2D.velocity.y);
            anim.SetBool("Grounded", grounded);
            if (jumping)
            {
                JumpAnim();
            }
            //grounded = controller.cont_collision_info.below;
            if (isSwinging)
            {
                //anim.SetTrigger("Grapple");
                anim.SetBool("Swinging", true);
            }
            else
            {
                anim.SetBool("Swinging", false);
            }

            //Certain point values give static buffs to the player, corresponding to the number of points already picked up
            if (powerBoost/15 > 0)
            {
                //Increase speed
                if (powerBoost/15 >= 7)
                {
                    speed = 8;
                    runSpeed = 16;
                }
                else if (powerBoost/15 >= 4)
                {
                    speed = 7;
                    runSpeed = 14;
                }
                else if (powerBoost/15 >= 1)
                {
                    speed = 6;
                    runSpeed = 12;
                }
                //Change health maximum
                if (powerBoost/15 >= 8)
                {
                    health_max = 8;
                    Debug.Log("Should have 8");
                }
                else if (powerBoost/15 >= 5)
                {
                    health_max = 7;
                }
                else if (powerBoost/15 >= 2)
                {
                    Debug.Log("Should have 6");
                    health_max = 6;
                }
                //Increase airdash speed
                if (powerBoost/15 >= 9)
                {
                    airdashSpeed = 36;
                }
                else if (powerBoost/15 >= 6)
                {
                    airdashSpeed = 30;
                }
                else if (powerBoost/15 >= 3)
                {
                    airdashSpeed = 24;
                }
                Debug.Log("Power Level = " + powerBoost/15);
            }

            powerBoost = points;
            //Add all powers cheat code
            if (Input.GetKeyDown(KeyCode.Y))
            {
                mPowerDict[reflector.name].active = true;
                mWeaponDict[freeze.name].active = true;
                tPowerDict[antiGrav.name].active = true;
                tWeaponDict[drill.name].active = true;
                Debug.Log("All powers added");
            }
            //tUtility.toString();
            //tWeapon.toString();
            //mUtility.toString();
            //mWeapon.toString();
        }
        if (Math.Abs(rig2D.velocity.y) > terminalVel)
        {
            if (rig2D.velocity.y > 0)
                rig2D.velocity = new Vector2(rig2D.velocity.x, terminalVel);
            else
                rig2D.velocity = new Vector2(rig2D.velocity.x, -terminalVel);
        }
        if (Math.Abs(rig2D.velocity.x) > terminalVel)
        {
            if (rig2D.velocity.x > 0)
                rig2D.velocity = new Vector2(terminalVel, rig2D.velocity.y);
            else
                rig2D.velocity = new Vector2(-terminalVel, rig2D.velocity.y);

        }
        anim.SetBool("Swinging", isSwinging);
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 14)
    //        Debug.Log("I HIT THE ENEMY");

    //}

    //Param 1 - The dictionary of available powers to switch to
    //Param 2 - The current power
    public bool AutomatedWalkToPosition(Vector3 pos)
    {
        if(this.transform.position.x < pos.x) {
            sprite.flipX = true; 
            directionalInput = new Vector3(1, 0, 0);            
            
        }
        else if(this.transform.position.x > pos.x)
        {
            sprite.flipX = false;
            directionalInput = new Vector3(-1, 0, 0);
            
        }
        controller.Move(directionalInput * speed * Time.deltaTime);
        if(Math.Abs(transform.position.x - pos.x) <= 1)
        {
            directionalInput.x = 0;
            WalkAnim(directionalInput);
            return true; 
        }
        WalkAnim(directionalInput);
        anim.SetBool("Walk", true);
        return false; 
    }

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
        powerNotif.newNotif(NewPower.name);
        return NewPower.active = true;
    }

    public void takeDamage(float damage, Vector2 knockDir)
    {
        
        //Debug.Log("invincible: " + invincible);
        if (!invincible)
        {
            health -= damage;
            hiScript.loseHealth(health);
            if (health <= 0)
            {
                //player has died
                Debug.Log("Player died!");
                //health = health_max;
                SavePlayer();
                SaveSceneData();
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
            if (isSwinging)
            {
                Debug.Log("release rope");
                rs.ResetRope();
            }
        }

    }

    public bool IncreaseHealth()
    {
        if(health != health_max)
        {
            health++;
            hiScript.gainHealth();
            return true;
        }
        return false;
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

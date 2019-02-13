using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Forces On Player ----------------
    public float gravity;
    public float fast_gravity;
    public float speed = 5f;
    public float airdashSpeed = 1.0f;
    public float runSpeed = 8f;
    

    public float jumpHeight = 10f;
    public float timeToJumpApex = .4f;

    public float airdashTime = 0;
    private bool hasAirdash = false;
    private Vector3 airdashDirection = new Vector3(0, 0, 0);

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
    private SpriteRenderer sprite;
    // -----------------------------

    //  States
    public bool alive; 



    //Calculate airdash direction here
    Vector3 calculateAirdashVector()
    {
        Vector3 vec;
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            vec = new Vector3(airdashSpeed * .65f, airdashSpeed * .65f, 0);
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            vec = new Vector3(-airdashSpeed * .65f, airdashSpeed * .65f, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            vec = new Vector3(airdashSpeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            vec = new Vector3(0, airdashSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            vec = new Vector3(-airdashSpeed, 0, 0);
        }
        else
        {
            vec = new Vector3(0, airdashSpeed, 0);
        }
        return vec;
    }

    Controller2D controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rig2D = GetComponent<Rigidbody2D>();
        //Gravity is directly proportional to given jump height, and disproportional to time it takes to reach maximum jump height
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        fast_gravity = gravity * 2;
        alive = true; 
        //How high you jump is directly proportional to gravity and the time it takes to reach max jump height
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.cont_collision_info.above || controller.cont_collision_info.below) //  Stops vertical movement if vertical collision detected
        {
            velocity.y = 0;
        }

        Vector2 directionalInput;
        if (!pa_inConvo)  // Can move while not in conversation
        {
            directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
        }
        else
        {
            directionalInput = new Vector2(0, 0);
        }

        //Flip Player Based on Direction
        if (directionalInput.x > 0)
        {
            sprite.flipX = true;
        }
        else if(directionalInput.x < 0)
        {
            sprite.flipX = false;
        }



        //On the ground, enable grounded only movement here
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
            if (Input.GetKey(KeyCode.S))
            {
                //Temp behavior
                //tf.localScale = new Vector3(5f, 2.5f, 5f);
                crouching = true; 
                speed = 0;
                runSpeed = 0;
            }
            else
            {
                //tf.localScale = new Vector3(5f, 5f, 5f);
                crouching = false; 
                speed = 5;
                runSpeed = 10;
            }
            //Run when holding P
            if (Input.GetKey(KeyCode.P))
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
                jumping = true; 
                velocity.y = jumpVelocity;
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
                airdashTime = .3f;
                this.airdashDirection = calculateAirdashVector();
            }

        }

        if (airdashTime > 0)
        {
            this.GetComponent<Transform>().localPosition += this.airdashDirection;
            airdashTime -= Time.deltaTime;
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; //  Gravity constant
            float targetX_velocity = directionalInput.x * speed;    //  Speed force added to horizontal velocity, no acceleration
                                                                    //  Damping/acceleration applied throught damping.
            velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below ? accelTime_ground : accelTime_air);
            //  Call to move function in controller2D class
            controller.Move(velocity * Time.deltaTime);
        }


        //Animation Update; 
        WalkAnim(directionalInput);
        CrouchAnim();
        JumpAnim();
        if (!alive)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Jump_Ascend", false);
        }
    }


    void WalkAnim(Vector2 input)
    {
        Debug.Log(input.x);
        if(input.x != 0)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Idle", false);
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
        Debug.Log(jumping);
        if (jumping && !controller.cont_collision_info.below)
        {
 
                anim.SetBool("Grounded", false);
                anim.SetBool("Jump_Ascend", true);
                anim.SetBool("Walk", false);
        }
        else if (velocity.y < 0 && !controller.cont_collision_info.below) 
        {
            jumping = false;
            //anim.SetBool("Grounded", true);
            anim.SetBool("Jump_Ascend", false);
        }
        else if(controller.cont_collision_info.below)
        {
            anim.SetBool("Jump_Ascend", false);

            anim.SetBool("Grounded", true);
        }

    }
}

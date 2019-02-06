using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Forces On Player ----------------
    public float gravity;
    public float fast_gravity;
    public float speed = 5f;
    public float airdashSpeed = 0.25f;
    public float runSpeed = 10f;


    public float jumpHeight = 8f;
    public float timeToJumpApex = .55f;

    public float airdashTime = 0;
    private bool hasAirdash = false;
    private Vector3 airdashDirection = new Vector3(0, 0, 0);

    private bool facingRight = true;

    public bool isSwinging = false;
    public Vector2 ropeHook;
    public float swingForce = 4f;

    float jumpVelocity;
    float velocX_smooth;

    float accelTime_air = .4f;
    float accelTime_ground = .1f;
    Vector3 velocity;
    // --------------------------------

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

    public float aimDirection()
    {
        float aim = 0f;
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            aim = 45f;
            facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            aim = 135f;
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            aim = -45f;
            facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            aim = -135f;
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            aim = 0f;
            facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            aim = 180f;
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.W))
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

    Controller2D controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        //Gravity is directly proportional to given jump height, and disproportional to time it takes to reach maximum jump height
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        fast_gravity = gravity * 2;

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

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //On the ground, enable grounded only movement here
        if (isSwinging)
        {
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
                    tf.localScale = new Vector3(5f, 2.5f, 5f);
                    speed = 0;
                    runSpeed = 0;
                }
                else
                {
                    tf.localScale = new Vector3(5f, 5f, 5f);
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
        }
        if (velocity.x > 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }
    }

}

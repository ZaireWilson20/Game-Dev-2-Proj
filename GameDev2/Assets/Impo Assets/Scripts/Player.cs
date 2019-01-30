using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Forces On Player ----------------
    float gravity;
    public float speed = 5f;

    public float jumpHeight = 10f;
    public float timeToJumpApex = .4f; 


    float jumpVelocity;
    float velocX_smooth;

    float accelTime_air = .4f;
    float accelTime_ground = .1f; 
    Vector3 velocity; 
    // --------------------------------


    Controller2D controller; 
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        //Gravity is directly proportional to given jump height, and disproportional to time it takes to reach maximum jump height
        gravity = -1*(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);   
        
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
        if (controller.cont_collision_info.below && Input.GetKeyDown(KeyCode.Space)) //  If grounded and spacebar is pressed, jump
        {
            velocity.y = jumpVelocity;
        }

        velocity.y += gravity * Time.deltaTime; //  Gravity constant
        float targetX_velocity = directionalInput.x * speed;    //  Speed force added to horizontal velocity, no acceleration

        //  Damping/acceleration applied throught damping.
        velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below?accelTime_ground:accelTime_air);

        //  Call to move function in controller2D class
        controller.Move(velocity * Time.deltaTime);
    }

}

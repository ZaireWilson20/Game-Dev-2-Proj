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

    public int health_max = 5;
    private int health = 5;
    public float attack = 1f;
    public float knockback = 5f;
    public float fireDelta = 0.5f;
    public float nextFire = 0.5f;
    private float fireTime = 0.0f;

    private GameObject newProjectile;
    public GameObject projectile;

    float jumpVelocity;
    float velocX_smooth;

    float accelTime_air = .4f;
    float accelTime_ground = .1f; 
    Vector3 velocity;
    private GameObject[] enemies;

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

        health = health_max;
        gameObject.SetActive(true);

        //create array containing all enemies in level
        //enemies = FindGameObjectsWithLayer(9);
    }


    /**
     * Helper function to find all GameObjects in a certain layer 
     */
    private GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] objArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> objList = new List<GameObject>();
        for (int i = 0; i < objArray.Length; i++)
        {
            if (objArray[i].layer == layer) {
                objList.Add(objArray[i]);
            }
        }        
        return objList.ToArray();
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


        //fire projectile if '1' key pressed and cooldown expired
        fireTime = fireTime + Time.deltaTime;
        if (Input.GetButton("Fire1") && fireTime > nextFire)
        {
            nextFire = fireTime + fireDelta;
            newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            newProjectile.SetActive(true);
            //newProjectile.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(Mathf.Sign(velocity.x),0,0));
            //newProjectile.velocity = transform.TransformDirection(Vector3.forward * 10);

            // create code here that animates the newProjectile
            //Debug.Log("Fire!");
            nextFire = nextFire - fireTime;
            fireTime = 0.0F;
        }

        velocity.y += gravity * Time.deltaTime; //  Gravity constant
        float targetX_velocity = directionalInput.x * speed;    //  Speed force added to horizontal velocity, no acceleration

        //  Damping/acceleration applied throught damping.
        velocity.x = Mathf.SmoothDamp(velocity.x, targetX_velocity, ref velocX_smooth, controller.cont_collision_info.below?accelTime_ground:accelTime_air);

        //  Call to move function in controller2D class
        controller.Move(velocity * Time.deltaTime);
    }

    /**
     * Could be adjusted to account for any shield effects 
     */
    public void takeDamage(int damage, float knockDir)
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
    }

}

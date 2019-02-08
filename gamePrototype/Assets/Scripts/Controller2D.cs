using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Controller2D : MonoBehaviour
{

    //collider attributes
    const float cont_skin_width = 0.015f;
    public int cont_H_raycount = 4;
    public int cont_V_raycount = 4;
    public float cont_max_climb_angle = 70f;   //  Max angle for slope that player is allowed to climb
    public float cont_max_desc_angle = 75f;     //  Maximum angle for slope that player is allowed to descend without dropping

    private float cont_H_rayspacing = 1f;
    private float cont_V_rayspacing = 1f;

    public CollisionInfo cont_collision_info; 

    public LayerMask cont_collision_mask;
    public float v_old = 0f;

    RaycOrigins cont_raycast_origins;

    SpriteRenderer sprite;

    BoxCollider2D cont_collider;
    
    struct RaycOrigins   //  Corners that raycasts will shoot initialize
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;     
    }

    public struct CollisionInfo //  Information about objects that this entity collides with
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope, descendingSlope;
        public float slope_angle, slope_angle_old;
        public Vector3 velocity_old; 

        public void Reset() //  Reset collision variables to init states
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false; 
            slope_angle_old = slope_angle;
            slope_angle = 0; 
        }
    }


    /*  Updates the position of the raycast origins relative to this entity */
    void UpdateRayO()
    {
        Bounds rayBounds = cont_collider.bounds;
        rayBounds.Expand(cont_skin_width * -2);

        cont_raycast_origins.bottomLeft = new Vector2(rayBounds.min.x, rayBounds.min.y);
        cont_raycast_origins.bottomRight = new Vector2(rayBounds.max.x, rayBounds.min.y);
        cont_raycast_origins.topLeft = new Vector2(rayBounds.min.x, rayBounds.max.y);
        cont_raycast_origins.topRight = new Vector2(rayBounds.max.x, rayBounds.max.y);

    }

    /*  Calculates spacing between horizontal and vertical rays */
    void CalculateRaySpacing()
    {
        Bounds rayBounds = cont_collider.bounds;
        rayBounds.Expand(cont_skin_width * -2);

        cont_H_raycount = Mathf.Clamp(cont_H_raycount, 2, int.MaxValue);
        cont_V_raycount = Mathf.Clamp(cont_V_raycount, 2, int.MaxValue);

        cont_H_rayspacing = rayBounds.size.y / (cont_H_raycount - 1);
        cont_V_rayspacing = rayBounds.size.x / (cont_V_raycount - 1);
    }


    //Function for moving character
    public void Move(Vector3 velocity)
    {

        cont_collision_info.Reset(); //  Info for collisions should be reset everytime player moves
        UpdateRayO(); //  Updates the positional vectors of the raycasts relative to object
        cont_collision_info.velocity_old = velocity;

        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if (velocity.x != 0)
        {
            horizontalCollsions(ref velocity); //Checks horizontal collisions if moving right or left
            //if moving right, don't flip sprite
            if (velocity.x > 0)
                sprite.flipY = false;
            //if moving left, flip sprite
            else
                sprite.flipY = true;
        }

        if (velocity.y != 0)
        {
            verticalCollisions(ref velocity); //Checks vertical collsions if moving up or down
        }

        transform.Translate(velocity); //Moves object
    }


    /*  Function for shooting out vertical raycasts and stopping vertical movement if collisions are detected   */
    void verticalCollisions(ref Vector3 velocity)  
    {
        float direction = Mathf.Sign(velocity.y);  //set raycast direction to sign of movement vector --- (0, -1, 0) is y facing down 
        float ray_length = Mathf.Abs(velocity.y) + cont_skin_width;    //set ray legnth to y velocity + skin width

        //  Checking ray hits/collision for all vertical rays based off of the direction you are moving
        for(int i = 0; i < cont_V_raycount; i++)
        {
            Vector2 raycastOrigin = (direction == -1) ? cont_raycast_origins.bottomLeft : cont_raycast_origins.topLeft; // raycast origin is bottom left if moving down, top left if moving up

            raycastOrigin += Vector2.right * (cont_V_rayspacing * i + velocity.x); //  moves raycast origin by current iteration of rayspacing taking horizontal movement into account
            
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.up * direction, ray_length, cont_collision_mask);
            Debug.DrawRay(raycastOrigin, Vector2.up * direction * ray_length, Color.green);
            //Debug.Log(hit);
            if (hit)
            {
                
                //  Init vertical collision info based on direction player is moving
                cont_collision_info.below = direction == -1;
                cont_collision_info.above = direction == 1;


                velocity.y = (hit.distance - cont_skin_width) * direction; // changes velocity.y to 0 if colliding
                if (hit.distance == 0) {
                    //Debug.Log("sunk");
                    velocity.y = -.05f * direction;
                }
               /* if (velocity.y != v_old)
                    Debug.Log(velocity.y);
                v_old = velocity.y; */
                //velocity.y = 0;
                //Debug.Log("Vertical velocity: " + velocity.y);
                ray_length = hit.distance; //  change ray length to first object collided with so that deeper collisions don't effect 

                if (cont_collision_info.climbingSlope)
                {
                    
                    velocity.x = velocity.y / Mathf.Tan(cont_collision_info.slope_angle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);   // Math for slope ascension
                }
            }
        }

        if (cont_collision_info.climbingSlope) // Handles brief pause when changing slopes. Corner case
        {
            float directionX = Mathf.Sign(velocity.x);
            ray_length = Mathf.Abs(velocity.x) + cont_skin_width;
            Vector2 rayOrigin = ((directionX == -1) ? cont_raycast_origins.bottomLeft : cont_raycast_origins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, ray_length, cont_collision_mask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != cont_collision_info.slope_angle)  
                {
                    velocity.x = (hit.distance - cont_skin_width) * directionX;
                    cont_collision_info.slope_angle = slopeAngle;
                }
            }

        }
    }

    /*  Function for shooting out horizontal raycasts and stopping horizontal movement if collisions are detected   */
    void horizontalCollsions(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.x);   
        float ray_length = Mathf.Abs(velocity.x) + cont_skin_width;

        //  Checking ray hits/collision for all horizontal rays based off of the direction you are moving
        for (int i = 0; i < cont_H_raycount; i++)
        {
            Vector2 raycastOrigin = (direction == -1) ? cont_raycast_origins.bottomLeft : cont_raycast_origins.bottomRight;
            raycastOrigin += Vector2.up * (cont_H_rayspacing * i);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right * direction, ray_length, cont_collision_mask);
            Debug.DrawRay(raycastOrigin, Vector2.right * direction * ray_length, Color.green);

            if (hit)
            {
                float slope_angle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 && slope_angle <= cont_max_climb_angle) //   Limits climbing based on platform slope
                {
                    if (cont_collision_info.descendingSlope)
                    {
                        cont_collision_info.descendingSlope = false;
                        velocity = cont_collision_info.velocity_old;
                    }
                    float distToSlope = 0;

                    if (slope_angle != cont_collision_info.slope_angle_old) // Removes space inbetween object and slope once object starts to climb slope
                    {
                        distToSlope = hit.distance - cont_skin_width;
                        velocity.x -= distToSlope * direction;
                    }
                    ClimbSlope(ref velocity, slope_angle);
                    velocity.x += distToSlope * direction;
                    
                }

                if (!cont_collision_info.climbingSlope || slope_angle > cont_max_climb_angle)
                {
                    cont_collision_info.left = direction == -1;
                    cont_collision_info.right = direction == 1;
                    velocity.x = (hit.distance - cont_skin_width) * direction;
                    ray_length = hit.distance;

                    if (cont_collision_info.climbingSlope) // Handling horizontal collisions while climbing slope
                    {
                        velocity.y = Mathf.Tan(cont_collision_info.slope_angle) * Mathf.Abs(velocity.x);
                    }
                }


            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float angle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVeloc_Y = Mathf.Sin(angle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVeloc_Y)
        {
            velocity.y = Mathf.Sin(angle * Mathf.Deg2Rad) * moveDistance;
            velocity.x = Mathf.Cos(angle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            cont_collision_info.below = true;
            cont_collision_info.climbingSlope = true;
            cont_collision_info.slope_angle = angle;
        }
    }


    //Function for descending slope. Modifies velocity based on the angle at which object is hitting slope
    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? cont_raycast_origins.bottomRight : cont_raycast_origins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, cont_collision_mask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= cont_max_desc_angle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - cont_skin_width <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVeloc = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVeloc;

                        cont_collision_info.slope_angle = slopeAngle;
                        cont_collision_info.descendingSlope = true;
                        cont_collision_info.below = true; 

                    }
                }
            }
        }
    }

    void Update()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        cont_collider = GetComponent < BoxCollider2D>();
        CalculateRaySpacing();
        sprite = GetComponent<SpriteRenderer>();
    }


}

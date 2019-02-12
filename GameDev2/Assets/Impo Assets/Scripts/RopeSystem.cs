using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Player player;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;

    private bool distanceSet;
    
    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    public float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();

    public float climbSpeed = 3f;
    public float maxRopeDistance = 6f;
    public float minRopeDistance = 1f;
    private bool isColliding;

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 5f * Mathf.Cos(aimAngle*Mathf.Deg2Rad);
        var y = transform.position.y + 5f * Mathf.Sin(aimAngle*Mathf.Deg2Rad);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void Awake()
    {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        player.aimDirection();
        playerPosition = transform.position;

        var aimDirection = Quaternion.Euler(0, 0, player.aimDirection()) * Vector2.right;

        if (!ropeAttached)
        {
            player.isSwinging = false;
            SetCrosshairPosition(player.aimDirection());
        }
        else
        {
            player.ropeHook = ropePositions.Last();
            player.isSwinging = true;
            crosshairSprite.enabled = false;
        }
        HandleInput(aimDirection);
        UpdateRopePositions();
        HandleRopeLength();
    }

    // 1
    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!ropeAttached)
            {
                // 2
                //if (ropeAttached) return;
                ropeRenderer.enabled = true;

                var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

                Debug.Log(hit.distance);
                // 3
                if (hit.collider != null)
                {
                    ropeAttached = true;
                    if (!ropePositions.Contains(hit.point))
                    {
                        // 4
                        // Jump slightly to distance the player a little from the ground after grappling to something.
                        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                        ropePositions.Add(hit.point);
                        ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                        ropeJoint.enabled = true;
                        ropeHingeAnchorSprite.enabled = true;
                    }
                }
                // 5
                else
                {
                    ropeRenderer.enabled = false;
                    ropeAttached = false;
                    ropeJoint.enabled = false;
                }
            }
            else if (ropeAttached)
            {
                ResetRope();
            }
        }
    }

    // 6
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        player.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
    }

    private void UpdateRopePositions()
    {
        // 1
        if (!ropeAttached)
        {
            return;
        }

        // 2
        ropeRenderer.positionCount = ropePositions.Count + 1;

        // 3
        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                // 4
                if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                // 6
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }

    private void HandleRopeLength()
    {
        // 1
        if (ropeJoint.distance <= maxRopeDistance && ropeJoint.distance >= minRopeDistance)
        {
            if (Input.GetAxis("Vertical") >= 1f && ropeAttached && !isColliding)
            {
                ropeJoint.distance -= Time.deltaTime * climbSpeed;
            }
            else if (Input.GetAxis("Vertical") < 0f && ropeAttached)
            {
                if (ropeJoint.distance+Time.deltaTime*climbSpeed > maxRopeDistance)
                    ropeJoint.distance = maxRopeDistance;
                else
                    ropeJoint.distance += Time.deltaTime * climbSpeed;
            }
        }
    }

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isColliding = false;
    }
}

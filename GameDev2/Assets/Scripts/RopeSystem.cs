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
    private GameObject[] GrapplePoints;
    private GameObject currGrapple;

    public float climbSpeed = 3f;
    public float maxRopeDistance = 6f;
    public float minRopeDistance = 1f;
    private bool isColliding;

    public float buffer = .5f;
    private float timeleft = 0f;
    private bool input = false;

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 5f * Mathf.Cos(aimAngle * Mathf.Deg2Rad);
        var y = transform.position.y + 5f * Mathf.Sin(aimAngle * Mathf.Deg2Rad);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void Awake()
    {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        GrapplePoints = GameObject.FindGameObjectsWithTag("GrapplePoint");
    }

    // Update is called once per frame
    void Update()
    {
        player.aimDirection();
        playerPosition = transform.position;
        currGrapple = ClosestGrapple();
        float xDist = currGrapple.transform.position.x - player.transform.position.x;
        float yDist = currGrapple.transform.position.y - player.transform.position.y;
        float aimDir = Mathf.Atan2(yDist, xDist)*Mathf.Rad2Deg;
        var aimDirection = Quaternion.Euler(0, 0, aimDir) * Vector2.right;

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
        //Debug.Log(currGrapple.name);
    }

    // 1
    private void HandleInput(Vector2 aimDirection)
    {
        //Debug.Log(aimDirection);
        if (Input.GetButtonDown("Utility") && player.powerset && player.tUtility.name == "grapple")
        {
            player.anim.SetTrigger("Grapple");
            // 2
            ropeRenderer.enabled = true;
            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

            // 5
            if (!ropeAttached)
            {
                input = true;
                timeleft = 0f;
            }
            //Debug.Log(hit.distance);
            // 3
            if (player.isSwinging)
            {
                ResetRope();
                input = false;
            }
            else  if (hit.collider != null)
            {
                player.anim.SetTrigger("OffGrapple");
                input = false;
                player.hasAirdash = true;
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
        }
        else if (input)
        {
            player.anim.SetTrigger("Grapple");
            if (!ropeAttached)
            {
                // 2
                //if (ropeAttached) return;
                ropeRenderer.enabled = true;

                var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

                //Debug.Log(hit.distance);
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
                    player.anim.SetTrigger("OffGrapple");
                    ropeRenderer.enabled = false;
                    ropeAttached = false;
                    ropeJoint.enabled = false;
                }
            }
            if (timeleft > buffer)
                input = false;
            timeleft += Time.deltaTime;
        }
        else if (Input.GetButtonDown("Jump") && ropeAttached)
        {
            ResetRope();
        }
    }

    // 6
    public void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        player.isSwinging = false;
        player.GetComponent<Rigidbody2D>().gravityScale = player.fallSpeed;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        player.anim.SetTrigger("OffGrapple");
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
            if (Input.GetAxis("Vertical") >= 0.5f && ropeAttached && !isColliding)
            {
                ropeJoint.distance -= Time.deltaTime * climbSpeed;
            }
            else if (Input.GetAxis("Vertical") < -0.5f && ropeAttached)
            {
                if (ropeJoint.distance + Time.deltaTime * climbSpeed > maxRopeDistance)
                    ropeJoint.distance = maxRopeDistance;
                else
                    ropeJoint.distance += Time.deltaTime * climbSpeed;
            }
        }
    }

    private GameObject ClosestGrapple()
    {
        GameObject Grapple = null;
        if (GrapplePoints.Length > 0)
            Grapple = GrapplePoints[0];
        for (int i = 1; i < GrapplePoints.Length; ++i)
        {
            if (Mathf.Abs(Vector2.Distance(player.transform.position, GrapplePoints[i].transform.position)) < Mathf.Abs(Vector2.Distance(player.transform.position, Grapple.transform.position)))
            {
                Grapple = GrapplePoints[i];
            }
        }
        return Grapple;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float cooldown = 10f;
    private float timer = 0f;
    private Color dark;
    private Color brighter;
    public GameObject laserBeam;
    private GameObject newLaser;
    public GameObject pair;
    public int maxRed = 230;
    LaserBeam lscript;
    public bool transmitter = true;
    private SpriteRenderer sprite;
    LaserController pairScript;

    // Start is called before the first frame update
    void Start()
    {
        lscript = laserBeam.GetComponent<LaserBeam>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        dark = brighter = sprite.color;
        Debug.Log("dark color: " + dark);
        if (transmitter)
        {
            pairScript = pair.GetComponent<LaserController>();
            pairScript.cooldown = cooldown;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= cooldown)
        {
            //turn on laser beam
            if (transmitter)
            {
                //draw laser beam from transmitter to receiver
                Vector3 midpoint = (transform.position + pair.transform.position) / 2;
                float angle = Mathf.Atan2(pair.transform.position.y - transform.position.y, pair.transform.position.x - transform.position.x) * 180 / Mathf.PI;
                float length = Vector3.Distance(transform.position, pair.transform.position);
                newLaser = Instantiate(laserBeam, midpoint, transform.rotation) as GameObject;
                newLaser.transform.localScale = new Vector3(length, 0.1f, 1);
                newLaser.transform.Rotate(new Vector3(0, 0, angle));
                newLaser.SetActive(true);
            }
            timer = -lscript.duration;
        }

        if (timer <= 0)
        {
            brighter = new Color(maxRed, 0, 0);
        }
        else if (timer - Time.deltaTime < Time.deltaTime)
        {
            brighter = dark;
        }
        else
        {
            brighter.r += ((maxRed - dark.r) / (cooldown / Time.deltaTime)) / 255; //increase r value linearly from start color to maxRed
            //Debug.Log("color: " + brighter.r);
            brighter.g = brighter.b = 0;    //is this necessary/desired?
        }
        sprite.color = brighter;
        //Debug.Log("color: " + sprite.color);
        timer += Time.deltaTime;
    }
}

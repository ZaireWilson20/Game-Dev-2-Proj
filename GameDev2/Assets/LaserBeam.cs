using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private int ct = 0;
    int flashCt;
    public float flashRate = 0.3f;
    public float offTime = 0.2f;
    private float timer = 0f;
    private float elapsedTime = 0f;
    private SpriteRenderer sprite;
    private Color clear;
    private Color here;
    public int damage = 1;
    private int dmg = 1;
    public int duration = 3;
    GameObject player;
    Player pscript;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        here = clear = sprite.color;
        clear.a = 0;
        ct = 0;
        flashCt = (int) (duration / flashRate);
        dmg = damage;
        player = GameObject.FindGameObjectWithTag("Player");
        pscript = player.GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            //beamed the player
            Debug.Log("Beamed the player!");
            pscript.takeDamage(dmg, Vector2.up);    //bounce the player up?
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= flashRate && ct < flashCt)
        {
            timer = 0;
            ct++;
            sprite.color = clear;
            dmg = 0;
            Physics2D.IgnoreLayerCollision(21, 13, true);
        }
        else if (timer >= offTime)
        {
            dmg = damage;
            sprite.color = here;
            Physics2D.IgnoreLayerCollision(21, 13, false);
        }
        if (elapsedTime > duration)
            gameObject.SetActive(false);
        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;
    }
}

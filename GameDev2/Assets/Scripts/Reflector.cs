using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    public float timer;
    public LayerMask mask;
    //public float unfreeze;
    //private float freezeTimer;
    private SpriteRenderer spr;

    void Awake()
    {
        mask = 15;
        timer = 2.5f;
        //freezeTimer = 0;
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    //    if (freezeTimer <= 0)
      //      freeze(false);
        timer -= Time.deltaTime;
        //freezeTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        GameObject obj = collision.gameObject;
        if (obj.layer == 16 || obj.layer == 15)
        {
            obj.GetComponent<Rigidbody2D>().velocity = -1*obj.GetComponent<Rigidbody2D>().velocity;
            obj.layer = mask;
            //Debug.Log(obj.GetComponent<Rigidbody2D>().velocity);
            Debug.Log(obj.layer);
        }
    }

    //public void freeze(bool val)
    //{
    //    if (val)
    //    {
    //        spr.color = new Color(0, 194, 255, 255);
    //        freezeTimer = unfreeze;
    //    }
    //    else
    //        spr.color = new Color(255, 255, 255, 255);
    //    //Debug.Log(val);
    //    //Debug.Log("Should be light blue");
    //}
}

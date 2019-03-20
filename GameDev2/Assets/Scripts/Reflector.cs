using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    public float timer;
    public LayerMask mask;

    void Awake()
    {
        mask = 16;
        timer = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        GameObject obj = collision.gameObject;
        if (obj.layer == 16 || obj.layer == 15)
        {
            //obj.GetComponent<Rigidbody2D>().velocity = -1*obj.GetComponent<Rigidbody2D>().velocity;
            obj.layer = mask;
            //Debug.Log(obj.GetComponent<Rigidbody2D>().velocity);
            Debug.Log(obj.layer);
        }
    }
}

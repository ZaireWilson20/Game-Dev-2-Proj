using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeState : MonoBehaviour
{

    public float unfreeze;
    private float freezeTimer;
    private SpriteRenderer spr;
    
    // Start is called before the first frame update
    void Start()
    {
        freezeTimer = 0;
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (freezeTimer <= 0)
            freeze(false);   
        freezeTimer -= Time.deltaTime;
    }

    public void freeze(bool val)
    {
        if (val)
        {
            spr.color = new Color(0, 194, 255, 255);
            freezeTimer = unfreeze;
        }
        else
            spr.color = new Color(255, 255, 255, 255);
        //Debug.Log(val);
        //Debug.Log("Should be light blue");
    }
}

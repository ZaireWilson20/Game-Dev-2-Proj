using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeState : MonoBehaviour
{

    public float unfreeze;
    public SimpleHostile hscript;
    private float freezeTimer;
    private SpriteRenderer spr;
    private int origLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        freezeTimer = 0;
        spr = GetComponent<SpriteRenderer>();
        hscript = GetComponentInParent<SimpleHostile>();
        origLayer = gameObject.layer;
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
            this.gameObject.layer = 20;
            if (hscript != null)
                hscript.enabled = false;
            spr.color = new Color(0, 194, 255, 255);
            freezeTimer = unfreeze;
        }
        else
        {
            this.gameObject.layer = origLayer;
            if (hscript != null)
                hscript.enabled = true;
            spr.color = new Color(255, 255, 255, 255);
        }
        //Debug.Log(val);
        //Debug.Log("Should be light blue");
    }
}

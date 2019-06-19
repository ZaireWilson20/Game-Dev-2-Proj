using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool magicPortal;
    Animator anim; 
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        if (magicPortal)
        {
            anim.SetBool("Magic", true);
        }
        else
        {
            anim.SetBool("Magic", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

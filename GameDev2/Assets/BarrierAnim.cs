using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierAnim : MonoBehaviour
{
    private Player player;
    bool playerOver;
    Animator anim;
    DoorController doorCont; 
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        doorCont = transform.parent.GetComponent<DoorController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("PlayerOver", true);
        doorCont.playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("PlayerOver", false);
        doorCont.playerInRange = false;
       
    }
}

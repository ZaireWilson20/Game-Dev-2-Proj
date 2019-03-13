using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public string name;
    public GameObject notifObj;
    public GameObject playerRef;
    private PlayerInventory playerInv; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown())
            notifObj.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            notifObj.SetActive(false);
        }
    }
}

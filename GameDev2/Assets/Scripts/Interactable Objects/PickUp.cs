using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public string name;
    public GameObject notifObj;
    public GameObject playerRef;
    private PlayerInventory playerInv;
    private bool playerIn; 

    
    // Start is called before the first frame update
    void Start()
    {
        playerInv = playerRef.GetComponent<PlayerInventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("I'm picking up shit");
                playerInv.AddToInv(this.GetComponent<PickUp>());
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            notifObj.SetActive(true);
            Debug.Log("in here");
            playerIn = true; 
            
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            notifObj.SetActive(false);
            playerIn = false; 
        }
    }

    //public void Copy(PickUp)
    //{

    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject playerObj;
    PlayerInventory pa_inv;
    bool playerInRange;
    public bool needsKey;
    public string keyNeeded;
    public GameObject notifObj; 
    // Start is called before the first frame update
    void Start()
    {
        pa_inv = playerObj.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Z) && !needsKey)
        {
            Debug.Log("Open Door");
        }
        else if(playerInRange && Input.GetKeyDown(KeyCode.Z) && needsKey)
        {
            if (!pa_inv.CheckInventory(keyNeeded))
            {
                Debug.Log("Missing " + keyNeeded);
            }
            else
            {
                Debug.Log("Door Open, " + keyNeeded + " in inventory.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = true;
            notifObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = false;
            notifObj.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject playerObj;
    PlayerInventory pa_inv;
    public string doorName;
    bool playerInRange;
    public bool needsKey;
    public string[] keysNeeded;
    public GameObject notifObj;
    public bool opened; 
    public string levelGoingTo;
    public LevelSwitch levelSwitch;
    private bool searched = false;
    public bool startActive = true;
    public Vector2 otherDoor;
    // Start is called before the first frame update
    void Start()
    {
        pa_inv = playerObj.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalControl.Instance.savedDoors.InList(doorName))
        {
            
            needsKey = GlobalControl.Instance.savedDoors.GetUnlocked(doorName);
            gameObject.SetActive(GlobalControl.Instance.savedDoors.GetActive(doorName));
        }
        else
        {
            //Debug.Log("Found door in list");
            
            GlobalControl.Instance.savedDoors.AddDoor(doorName, needsKey, startActive);
            gameObject.SetActive(startActive);
        }
        if (playerInRange && Input.GetButtonDown("Pickup") && !needsKey)
        {
            //Debug.Log("Open Door");
            opened = true; 
            OpenDoor();
        }
        else if(playerInRange && Input.GetButtonDown("Pickup") && needsKey)
        {
            bool hasKey = false; 
            foreach (string k in keysNeeded) {
                hasKey = pa_inv.CheckInventory(k);
                if (!hasKey) { break; }
            }
            if(hasKey)
            {
                //Debug.Log("Door Open, " +  + " in inventory.");
                opened = true; 
                OpenDoor();
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

    public void OpenDoor()
    {
        //if (Input.GetButtonDown("Pickup"))
        //{
        playerObj.GetComponent<Player>().spawnPosition = otherDoor;
        Debug.Log(otherDoor);
        levelSwitch.FadeToLevel(levelGoingTo);
        //}
    }
}

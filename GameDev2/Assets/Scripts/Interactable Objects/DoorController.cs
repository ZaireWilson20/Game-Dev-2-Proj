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
    public string keyNeeded;
    public GameObject notifObj;
    public bool opened; 
    public string levelGoingTo;
    public LevelSwitch levelSwitch;
    private bool searched = false;
    public bool startActive = true; 
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
        if (playerInRange && Input.GetKeyDown(KeyCode.Z) && !needsKey)
        {
            //Debug.Log("Open Door");
            opened = true; 
            OpenDoor();
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
        levelSwitch.FadeToLevel(levelGoingTo);
    }
}

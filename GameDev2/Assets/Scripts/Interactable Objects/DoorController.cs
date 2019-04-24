using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject playerObj;
    PlayerInventory pa_inv;
    public string doorName;
    public bool playerInRange;
    public bool needsKey;
    public bool isBarrier; 
    public string[] keysNeeded;
    public GameObject notifObj;
    public bool opened; 
    public string levelGoingTo;
    public string spawnName; 
    public LevelSwitch levelSwitch;
    private Player player; 
    private bool searched = false;
    public bool startActive = true;
    public string nextLevelSpawnPoint; 
    public Vector2 otherDoor;
    Animator anim; 
    // Start is called before the first frame update
    void Start()
    {
        pa_inv = playerObj.GetComponent<PlayerInventory>();
        player = playerObj.GetComponent<Player>();
        anim = GetComponent<Animator>();
        if (GlobalControl.Instance.savedDoors.InList(doorName))
        {
            Debug.Log(doorName + " is in list");
            needsKey = GlobalControl.Instance.savedDoors.GetUnlocked(doorName);
            gameObject.SetActive(GlobalControl.Instance.savedDoors.GetActive(doorName));
            if (isBarrier)
            {
                opened = GlobalControl.Instance.savedDoors.GetBarrierOpen(doorName);
            }
        }
        else
        {
            Debug.Log(doorName + " not in list");

            GlobalControl.Instance.savedDoors.AddDoor(doorName, needsKey, startActive);
            gameObject.SetActive(startActive);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInRange && Input.GetButtonDown("Pickup") && !needsKey && !isBarrier)
        {
            Debug.Log("No key needed");
            opened = true; 
            OpenDoor();
        }
        else if(playerInRange && Input.GetButtonDown("Pickup") && needsKey && !isBarrier)
        {
            bool hasKey = false; 
            foreach (string k in keysNeeded) {
                hasKey = pa_inv.CheckInventory(k);
                Debug.Log("Has key: " + k + ": " + hasKey);
                if (!hasKey) { break; }
            }
            if(hasKey)
            {
                //Debug.Log("Door Open, " +  + " in inventory.");
                opened = true; 
                OpenDoor();
            }
        }
        else if(isBarrier && opened)
        {
            anim.SetTrigger("BarrierOpened");
        }
        else if(playerInRange && Input.GetButtonDown("Pickup") && needsKey && isBarrier)
        {
            bool hasKey = false;
            foreach (string k in keysNeeded)
            {
                hasKey = pa_inv.CheckInventory(k);
                Debug.Log("Player has: " + k + ": " + hasKey);
                if (!hasKey) {
                    Debug.Log("No Key");
                    break; }
            }
            if (hasKey)
            {
                //Debug.Log("Door Open, " +  + " in inventory.");
                opened = true;
                anim.SetTrigger("OpenBarrier");
                GlobalControl.Instance.savedDoors.SetBarrierOpen(doorName);
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isBarrier)
        {
            playerInRange = true;
            notifObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isBarrier)
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
        //player.pointToSpawn = nextLevelSpawnPoint;
        Debug.Log(otherDoor);
        player.SavePlayer();
        player.SaveSceneData();
        levelSwitch.FadeToLevel(levelGoingTo);
        //}
    }
}

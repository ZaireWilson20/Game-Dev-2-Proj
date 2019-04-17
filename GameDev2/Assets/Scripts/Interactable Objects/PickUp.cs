using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public string _name;
    public GameObject notifObj;
    public GameObject playerRef;
    private PlayerInventory playerInv;
    private Player playerScript;
    private bool playerIn;
    public bool isKey;
    public bool isPowerUp;
    private GainedUpgrade notif;

    
    // Start is called before the first frame update
    void Start()
    {
        playerInv = playerRef.GetComponent<PlayerInventory>();
        playerScript = playerRef.GetComponent<Player>();
        notif = FindObjectOfType<GainedUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn)
        {
            if (Input.GetButtonDown("Pickup"))
            {
                if (!isPowerUp)
                {
                    Debug.Log("I'm picking up shit");
                    playerInv.AddToInv(this.GetComponent<PickUp>());
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("I'm powering up!");
                    bool foundItem = false;
                    foreach(KeyValuePair<string,Power> pow in playerScript.tPowerDict)
                    {
                        if (pow.Key == _name) 
                        {
                            pow.Value.setActive();
                            foundItem = true;
                            continue;
                        }
                    }

                    if (!foundItem)
                    {
                        foreach(KeyValuePair<string, Power> pow in playerScript.tWeaponDict)
                        {
                            if (pow.Key == _name)
                            {
                                pow.Value.setActive();
                                foundItem = true;
                                continue;
                            }
                        }
                    }

                    if (!foundItem)
                    {
                        foreach (KeyValuePair<string, Power> pow in playerScript.mWeaponDict)
                        {
                            if (pow.Key == _name)
                            {
                                pow.Value.setActive();
                                Debug.Log(pow.Key);
                                pow.ToString();
                                foundItem = true;
                                continue;
                            }
                        }
                    }

                    if (!foundItem)
                    {
                        foreach (KeyValuePair<string, Power> pow in playerScript.mPowerDict)
                        {
                            if (pow.Key == _name)
                            {
                                pow.Value.setActive();
                                foundItem = true;
                                continue;
                            }
                        }
                    }

                    if (foundItem)
                    {
                        notif.newNotif(_name);
                        Destroy(this.gameObject);
                    }
                }
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

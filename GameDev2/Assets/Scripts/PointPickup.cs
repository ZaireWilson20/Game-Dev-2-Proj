using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour
{
    private Player player;
    private GainedUpgrade upNotif;
    public int PickupValue = 1;
    public bool pickedup = false;
    public string id = "";
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalControl.Instance.savedPickups.InTable(this))
            pickedup = (bool) GlobalControl.Instance.savedPickups.pickupTable[id];
        else
        {
            //Debug.Log("Added a point");
            GlobalControl.Instance.savedPickups.pickupTable.Add(id, pickedup);
        }
        if (pickedup)
            this.gameObject.SetActive(false);
        upNotif = FindObjectOfType<GainedUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player.points/15 < (player.points+PickupValue)/15)
            {
                if (((player.points+PickupValue)/15)%3 == 0)
                {
                    upNotif.newNotif("Increased airdash distance");
                }
                else if (((player.points + PickupValue) / 15) % 3 == 2)
                {
                    upNotif.newNotif("Increase health");
                    GlobalControl.Instance.savedPlayer.playerHealthCap++;
                    player.GetComponent<Player>().IncreaseHealth();
                    player.SetHealthMax();
                }
                else if (((player.points + PickupValue) / 15) % 3 == 1)
                {
                    upNotif.newNotif("Speed boost");
                }
            }
            player.points += PickupValue;
            Debug.Log(player.points);
            pickedup = true;
            GlobalControl.Instance.savedPickups.pickupTable[id] = true;
            this.gameObject.SetActive(false);
        }
    }
}

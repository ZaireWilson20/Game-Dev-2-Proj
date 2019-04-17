using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour
{
    private Player player;
    private GainedUpgrade upNotif;
    public int PickupValue = 1;
    // Start is called before the first frame update
    void Start()
    {
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
                    upNotif.newNotif("airdash");
                }
                else if (((player.points + PickupValue) / 15) % 3 == 2)
                {
                    upNotif.newNotif("health");
                }
                else if (((player.points + PickupValue) / 15) % 3 == 1)
                {
                    upNotif.newNotif("speed");
                }
            }
            player.points += PickupValue;
            Debug.Log(player.points);
            Destroy(this.gameObject);
        }
    }
}

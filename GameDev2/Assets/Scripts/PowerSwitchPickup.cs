using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchPickup : MonoBehaviour
{
    private Player player;
    private GainedUpgrade notif;
    public GameObject pickupableIcon;
    private bool playerIn = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        notif = FindObjectOfType<GainedUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn)
            if (Input.GetButtonDown("Pickup"))
            {
                player.canSwitch = true;
                Debug.Log("Can now switch sides");
                Destroy(this.gameObject);
                notif.newNotif("switch side");
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIn = true;
            pickupableIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIn = false;
            pickupableIcon.SetActive(false);
        }
    }
}
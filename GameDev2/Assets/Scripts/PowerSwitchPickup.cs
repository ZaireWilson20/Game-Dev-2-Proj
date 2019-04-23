using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchPickup : MonoBehaviour
{
    private Player player;
    private GainedUpgrade notif;
    public GameObject pickupableIcon;
    private bool playerIn = false;
    public bool PickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        PickedUp = GlobalControl.Instance.savedPickups.powerSwitch.PickedUp;
        player = FindObjectOfType<Player>();
        notif = FindObjectOfType<GainedUpgrade>();
        if (PickedUp)
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn)
            if (Input.GetButtonDown("Pickup"))
            {
                player.canSwitch = true;
                GlobalControl.Instance.savedPickups.powerSwitch.PickedUp = true;
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
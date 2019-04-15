﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchPickup : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {

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
            player.canSwitch = true;
            Debug.Log("Can now switch sides");
            Destroy(this.gameObject);
        }
    }
}
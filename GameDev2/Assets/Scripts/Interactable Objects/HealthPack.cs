using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private Player player;
    bool playerOver;
    Animator anim; 
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOver && Input.GetButtonDown("Pickup"))
        {
            bool healthIncrease = player.IncreaseHealth();
            if (healthIncrease)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("At Full Health");
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("PlayerOver", true);
        if(collision.tag == "Player")
        {
            playerOver = true; 
            player = collision.gameObject.GetComponent<Player>(); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("PlayerOver", false);
        playerOver = false; 
    }
}

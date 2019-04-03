using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    // Start is called before the first frame update
    private Player playerObj;
    public bool goingRight;
    private bool playerOnObj; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObj != null && playerOnObj)
        {
            playerObj.velocity = new Vector3(playerObj.velocity.x + .5f * (goingRight ? 1 : -1), playerObj.velocity.y, playerObj.velocity.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerObj = collision.GetComponent<Player>();
            playerOnObj = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerOnObj = false;
        }
    }

}

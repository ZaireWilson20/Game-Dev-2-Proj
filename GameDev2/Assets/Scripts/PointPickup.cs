using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickup : MonoBehaviour
{
    private Player player;
    public int PickupValue = 1;
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
            player.points += PickupValue;
            Debug.Log(player.points);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private List<PickUp> completeInventory;
   
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
        if(collision.gameObject.tag == "Pick Up")
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                PickUp tempPick = collision.gameObject.GetComponent<PickUp>();
                completeInventory.Add(tempPick);
                Destroy(collision.gameObject);


            }
        }
    }

    public void AddToInv(PickUp item)
    {
        PickUp tempPickUp = item;
        completeInventory.Add(tempPickUp);
    }
}

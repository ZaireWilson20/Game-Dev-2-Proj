using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<InventoryItem> completeInventory = new List<InventoryItem>();
    public int _size = 0; 
    
    // Start is called before the first frame update
    void Start()
    {
        completeInventory = GlobalControl.Instance.savedPlayer.inventory; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (InventoryItem item in completeInventory)
            {
                Debug.Log("Player has: " + item._name);
            }
        }
    }

    public void AddToInv(PickUp item)
    {
        Debug.Log(item._name);
        InventoryItem tempPickUp = new InventoryItem();
        tempPickUp.CopyPickUp(item);
        completeInventory.Add(tempPickUp);
        _size = completeInventory.Count;
        GlobalControl.Instance.savedPlayer.inventory = completeInventory;
    }

    public bool CheckInventory(string name)
    {
        
        foreach(InventoryItem item in completeInventory)
        {
            if(name == item._name)
            {
                return true; 
            }
        }
        return false; 
    }
   
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<InventoryItem> completeInventory = new List<InventoryItem>();
    public int _size = 0; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInv(PickUp item)
    {
        InventoryItem tempPickUp = new InventoryItem();
        tempPickUp.CopyPickUp(item);
        completeInventory.Add(tempPickUp);
        _size = completeInventory.Count;
        
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public string _name;
    public Sprite invSprite; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CopyPickUp(PickUp item)
    {
        _name = item._name;
        invSprite = item.GetComponent<SpriteRenderer>().sprite;
    }
}

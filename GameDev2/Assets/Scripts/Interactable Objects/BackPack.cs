using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPack : MonoBehaviour
{
    public Image[] allItems;
    public GameObject p_inventory;
    private PlayerInventory invScript;
    private int count;
    public GameObject menu;

    private void Close()
    {
        menu.SetActive(true);
        menu.GetComponentInChildren<PauseMenu>().backToMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        invScript = p_inventory.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invScript._size != allItems.Length && invScript._size != 0)
        {
            count = invScript._size;
            allItems[count - 1].gameObject.SetActive(true);
            allItems[count - 1].sprite = invScript.completeInventory[invScript._size - 1].invSprite;          
        }

        if (Input.GetButtonDown("Click"))
        {
            Close();
        }

    }
}

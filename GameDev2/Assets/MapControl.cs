using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Close()
    {
        menu.SetActive(true);
        menu.GetComponentInChildren<PauseMenu>().backToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Click"))
        {
            Close();
        }
    }
}

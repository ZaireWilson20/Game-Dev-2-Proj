using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlDisplay : MonoBehaviour
{
    public GameObject menu;
    public bool titleScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Close()
    {
        if (!titleScreen)
        {
            menu.SetActive(true);
            menu.GetComponentInChildren<PauseMenu>().backToMenu();
        }
        else
        {
            SceneManager.LoadScene("TitlescreenNew", LoadSceneMode.Single);
        }
            
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

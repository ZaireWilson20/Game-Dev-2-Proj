using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClick : MonoBehaviour
{
    public GameObject ui_Inventory;
    public GameObject menu;
    public GameObject ui_Skills;
    public GameObject ui_Quit;
    public GameObject gameManager;
    private Player_UI_Input gmScript;

    // Start is called before the first frame update
    void Start()
    {
        gmScript = gameManager.GetComponent<Player_UI_Input>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateInv()
    {
        Debug.Log("Pressed Inv");
        if (ui_Inventory.activeSelf)
        {
            ui_Inventory.SetActive(false);
        }
        else
        {
            ui_Inventory.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void activateSkills()
    {
        if (ui_Skills.activeSelf)
        {
            ui_Skills.SetActive(false);
        }
        else
        {
            ui_Skills.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        gmScript.ResumeGame();
    }

    public void backToMenu()
    {
        ui_Inventory.SetActive(false);
        ui_Quit.SetActive(false);
        menu.SetActive(true);
        ui_Skills.SetActive(false);
    }

    public void QuitGame()
    {
        ui_Quit.SetActive(true);
        menu.SetActive(false);
    }

    public void YesQuit()
    {
        Application.Quit();
    }

    public void NoQuit()
    {
        backToMenu();
    }
}

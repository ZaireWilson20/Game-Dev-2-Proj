using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI_Input : MonoBehaviour
{

    public GameObject menu;
    public GameObject map;
    public GameObject invObj;

    //  Game Manager
    public GameObject gameManagerObj;
    private GameState gameManager;


    private enum ui_state
    {
        game,
        map, 
        backpack,
        menu 
    }
    
    private ui_state currentState = ui_state.game;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        map.SetActive(false);
        invObj.SetActive(false);
        gameManager = gameManagerObj.GetComponent<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        //Map Input
        if (Input.GetKeyDown(KeyCode.Tab) && currentState == ui_state.game)
        {
            currentState = ui_state.map;
            map.SetActive(true);
            gameManager.paused = true; 
        }
        else if (Input.GetButtonDown("Pause") && currentState == ui_state.game)
        {
            currentState = ui_state.menu;
            menu.SetActive(true);
            gameManager.paused = true;

        }
        else if(Input.GetButtonDown("Inventory") && currentState == ui_state.game)
        {
            currentState = ui_state.backpack;
            invObj.SetActive(true);
            gameManager.paused = true; 
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && currentState == ui_state.map)
        {
            map.SetActive(false);
            currentState = ui_state.game;
            gameManager.paused = false;
        }
        else if (Input.GetButtonDown("Pause") && currentState == ui_state.menu)
        {
            menu.SetActive(false);
            currentState = ui_state.game;
            gameManager.paused = false;
        }
        else if (Input.GetButtonDown("Inventory") && currentState == ui_state.backpack)
        {
            invObj.SetActive(false);
            currentState = ui_state.game;
            gameManager.paused = false;
        }



    }

    public void ResumeGame()
    {
        menu.SetActive(false);
        currentState = ui_state.game;
        gameManager.paused = false;
    }
}

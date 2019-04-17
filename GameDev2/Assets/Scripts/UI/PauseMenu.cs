using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Button[,] buttons = new Button[3, 2];
    private int row = 0;
    private int column = 0;

    public GameObject ui_Inventory;
    public GameObject menu;
    public GameObject ui_Skills;
    public GameObject ui_Quit;
    [SerializeField]
    private Button resume;
    [SerializeField]
    private Button map;
    [SerializeField]
    private Button inventory;
    [SerializeField]
    private Button skills;
    [SerializeField]
    private Button sound;
    [SerializeField]
    private Button quit;
    public Button yesQuit;
    public Button noQuit;
    private bool confirm = false;
    private Button[] confirmButtons = new Button[2];
    private Button selected;
    //public GameObject gameManager;
    //private Player_UI_Input gmScript;

    public float CycleDelay = 0.0f;     //time between button cycling
    private float timer = 0.0f;

    public Color standard;
    public Color highlighted;

    // Start is called before the first frame update
    void Start()
    {
        //gmScript = gameManager.GetComponent<Player_UI_Input>();
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject g in tempList) { 
        resume = GameObject.Find("Resume").GetComponent<Button>();
        resume.GetComponent<Image>().color = highlighted;
        map = GameObject.Find("MapButton").GetComponent<Button>();
        map.GetComponent<Image>().color = standard;
        inventory = GameObject.Find("InventoryPause").GetComponent<Button>();
        inventory.GetComponent<Image>().color = standard;
        skills = GameObject.Find("Skills").GetComponent<Button>();
        skills.GetComponent<Image>().color = standard;
        sound = GameObject.Find("Sound").GetComponent<Button>();
        sound.GetComponent<Image>().color = standard;
        quit = GameObject.Find("Quit").GetComponent<Button>();
        quit.GetComponent<Image>().color = standard;
            }
        //noQuit = GameObject.Find("NoQuit").GetComponent<Button>();
        //yesQuit = GameObject.Find("YesQuit").GetComponent<Button>();
        selected = resume;
        //Debug.Log("resume button: " + resume.gameObject.name);
       

        buttons[0, 0] = resume;
        buttons[0, 1] = map;
        buttons[1, 0] = inventory;
        buttons[1, 1] = skills;
        buttons[2, 0] = sound;
        buttons[2, 1] = quit;

        //Highlight(selected);

        confirmButtons[0] = noQuit;
        confirmButtons[1] = yesQuit;
    }

//    public void QuitGame()
//    {
//        Debug.Log("leaving game...");
//        // save any game data here
//#if UNITY_EDITOR
//        // Application.Quit() does not work in the editor so
//        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//         Application.Quit();
//#endif
//    }

    void Highlight(Button b)
    {
        //Debug.Log(b.gameObject.name);
        if (!confirm)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    //Debug.Log("(" + i + ", " + j + ")");
                    //Debug.Log("button: " + buttons[i, j].gameObject.name);
                    if (buttons[i, j] == b)
                        buttons[i, j].GetComponent<Image>().color = highlighted;
                    else
                        buttons[i, j].GetComponent<Image>().color = standard;
                }
            }
        } else
        {
            foreach (Button btn in confirmButtons)
            {
                if (btn == b)
                    btn.GetComponent<Image>().color = highlighted;
                else
                    btn.GetComponent<Image>().color = standard;
            }
        }

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
            //menu.SetActive(false);
        }
    }

    //public void ResumeGame()
    //{
    //    gmScript.ResumeGame();
    //}

    public void backToMenu()
    {
        confirm = false;
        selected = resume;
        //Highlight(selected);
        ui_Inventory.SetActive(false);
        ui_Quit.SetActive(false);
        menu.SetActive(true);
        ui_Skills.SetActive(false);
    }

    public void QuitGame()
    {
        //confirm = true;
        //ui_Quit.SetActive(true);
        Debug.Log("leaving game...");
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        //menu.SetActive(false);
    }

    public void YesQuit()
    {
        /*
        Debug.Log("leaving game...");
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
*/
    }

    public void NoQuit()
    {
        backToMenu();
    }

    void CycleUp(bool up)
    {
        if (up && row > 0)
            row--;
        else if (!up && row < 2)
            row++;
        selected = buttons[row, column];
        //Highlight(selected);
    }

    void CycleRight(bool right)
    {
        if (!right && column > 0)
            column--;
        else if (right && column < 1)
            column++;
        selected = buttons[row, column];
        //Highlight(selected);
    }

    private void Update()
    {
        //cycle up
        if (!confirm)
        {
            if (Input.GetAxisRaw("Vertical") > 0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    CycleUp(true);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < -0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    CycleUp(false);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    CycleRight(true);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    CycleRight(false);
                }
            }
        } else
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    selected = yesQuit;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                if (timer >= CycleDelay)
                {
                    timer = 0.0f;
                    selected = noQuit;
                }
            }
            //Highlight(selected);
        }

        timer += Time.deltaTime;

        if (Input.GetButtonDown("Click"))
        {
            //selected.onClick.Invoke();
        }
    }
}
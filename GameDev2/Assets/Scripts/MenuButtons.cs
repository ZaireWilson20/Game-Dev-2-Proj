using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuButtons : MonoBehaviour
{
    Button[] vertCycle = new Button[4];
    Button[] horizCycle = new Button[4];
    Button play, quit, options, controls, selected;
    public GameObject ui_Controls;
    int vertIndex = 0;
    int horizIndex = 0;
    int debug_ct = 0;
    public string nextScene;

    public float CycleDelay = 0.0f;     //time between button cycling
    private float timer = 0.0f;

    public Sprite standard;
    public Sprite highlighted;

    //Start is called before the first frame update
    void Start()
    {
        //collect the buttons
        play = GameObject.Find("PlayButton").GetComponent<Button>();
        play.GetComponent<Image>().sprite= highlighted;
        quit = GameObject.Find("QuitButton").GetComponent<Button>();
        quit.GetComponent<Image>().sprite = standard;
        options = GameObject.Find("OptionsButton").GetComponent<Button>();
        options.GetComponent<Image>().sprite = standard;
        controls = GameObject.Find("ControlsButton").GetComponent<Button>();
        controls.GetComponent<Image>().sprite = standard;
        selected = play;

        //compile vertical ordering of buttons
        vertCycle[0] = play;
        vertCycle[1] = controls;
        vertCycle[2] = options;
        vertCycle[3] = quit;

        //compile horizontal ordering of buttons
        //horizCycle[0] = play;
        //horizCycle[1] = options;
        //horizCycle[2] = quit;
        //horizCycle[3] = controls;

    }

    public void activateControls()
    {
        SceneManager.LoadScene("ControlsScreen", LoadSceneMode.Single);
    }

    public void LoadNextScene()
    {
        Debug.Log("start game");
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("leaving game...");
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    void Highlight(Button b)
    {
        foreach(Button btn in vertCycle) 
        {
            if (btn == b)
                btn.GetComponent<Image>().sprite = highlighted;
            else
                btn.GetComponent<Image>().sprite = standard;
        }
    }

    void CycleUp(bool up)
    {
        if (up && vertIndex > 0)
            vertIndex--;
        else if (!up && vertIndex < 3)
            vertIndex++;
        selected = vertCycle[vertIndex];
        Highlight(selected);
    }

    void Update()
    {
        //cycle up
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (timer >= CycleDelay)
            {
                timer = 0.0f;
                CycleUp(true);
            }
        } else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (timer >= CycleDelay)
            {
                timer = 0.0f;
                CycleUp(false);
            }
        }
        
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Click"))
        {
            selected.onClick.Invoke();
        }
    }
}

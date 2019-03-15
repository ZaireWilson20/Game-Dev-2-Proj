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
    int vertIndex = 0;
    int horizIndex = 0;
    int debug_ct = 0;

    public float CycleDelay = 0.2f;     //time between button cycling
    private float timer = 0.0f;

    public Color standard;
    public Color highlighted;

    //Start is called before the first frame update
    void Start()
    {
        //collect the buttons
        play = GameObject.Find("PlayButton").GetComponent<Button>();
        play.GetComponent<Image>().color= highlighted;
        quit = GameObject.Find("QuitButton").GetComponent<Button>();
        quit.GetComponent<Image>().color = standard;
        options = GameObject.Find("OptionsButton").GetComponent<Button>();
        options.GetComponent<Image>().color = standard;
        controls = GameObject.Find("ControlsButton").GetComponent<Button>();
        controls.GetComponent<Image>().color = standard;
        selected = play;

        //compile vertical ordering of buttons
        vertCycle[0] = play;
        vertCycle[1] = quit;
        vertCycle[2] = options;
        vertCycle[3] = controls;

        //compile horizontal ordering of buttons
        horizCycle[0] = play;
        horizCycle[1] = options;
        horizCycle[2] = quit;
        horizCycle[3] = controls;

    }

    public void LoadNextScene()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("Hub", LoadSceneMode.Single);
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
                btn.GetComponent<Image>().color = highlighted;
            else
                btn.GetComponent<Image>().color = standard;
        }
    }

    void Update()
    {
        //cycle right
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            if (timer >= CycleDelay)
            {
                debug_ct++;
                Debug.Log("right");
                //Debug.Log(debug_ct);
                timer = 0.0f;
                horizIndex++;
                if (horizIndex > 3)
                    horizIndex = 0;
                vertIndex += 2;
                if (vertIndex > 3)
                    vertIndex = 0;
                //highlight button at horizCycle[horixIndex]
                Debug.Log(horizIndex);
                //horizCycle[horizIndex].Select();
                Highlight(horizCycle[horizIndex]);
                selected = horizCycle[horizIndex];
                Debug.Log(selected);
            }
        }

        //cycle left
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            if (timer >= CycleDelay)
            {
                debug_ct++;
                //Debug.Log(debug_ct);
                Debug.Log("left");
                timer = 0.0f;
                horizIndex--;
                vertIndex -= 2;
                if (horizIndex < 0)
                    horizIndex = 3;
                if (vertIndex < 0)
                    vertIndex = 3;
                //highlight button at horizCycle[horixIndex]
                //horizCycle[horizIndex].Select();
                Highlight(horizCycle[horizIndex]);
                selected = horizCycle[horizIndex];
            }
        }

        //cycle up
        else if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (timer >= CycleDelay)
            {
                debug_ct++;
                //Debug.Log(debug_ct);
                Debug.Log("up");
                timer = 0.0f;
                vertIndex--;
                horizIndex -= 2;
                if (vertIndex < 0)
                    vertIndex = 3;
                if (horizIndex < 0)
                    horizIndex = 3;
                //highlight button at vertCycle[vertIndex]
                //vertCycle[vertIndex].Select();
                Highlight(vertCycle[vertIndex]);
                selected = vertCycle[vertIndex];
            }
        }

        //cycle down
        //if (Input.GetAxis("Vertical") < 0f  && Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(Input.GetAxis("Vertical")))
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (timer >= CycleDelay)
            {
                debug_ct++;
                //Debug.Log(debug_ct);
                Debug.Log("down");
                timer = 0.0f;
                vertIndex++;
                horizIndex += 2;
                if (vertIndex > 3)
                    vertIndex = 0;
                if (horizIndex > 3)
                    horizIndex = 0;
                //highlight button at vertCycle[vertIndex]
                //vertCycle[vertIndex].Select();
                Highlight(vertCycle[vertIndex]);
                selected = vertCycle[vertIndex];
            }
        }
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Click"))
        {
            selected.onClick.Invoke();
        }
    }
}

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

        //cycle right
        //if (Input.GetAxisRaw("Horizontal") > 0.5f)
        //{
        //    if (timer >= CycleDelay)
        //    {
        //        debug_ct++;
        //        Debug.Log("right");
        //        //Debug.Log(debug_ct);
        //        timer = 0.0f;
        //        if (selected == play)
        //        {
        //            Highlight(options);
        //            selected = options;
        //        } else if (selected == quit)
        //        {
        //            Highlight(controls);
        //            selected = controls;
        //        }

        //        //horizIndex++;
        //        //if (horizIndex > 3)
        //        //    horizIndex = 0;
        //        //vertIndex += 2;
        //        //if (vertIndex > 3)
        //        //    vertIndex = 0;

        //        //Debug.Log(horizIndex);
        //        //horizCycle[horizIndex].Select();
        //        //highlight button at horizCycle[horixIndex]
        //        //Highlight(horizCycle[horizIndex]);
        //        //selected = horizCycle[horizIndex];
        //        //Debug.Log(selected);
        //    }
        //}

        //cycle left
        //else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        //{
        //    if (timer >= CycleDelay)
        //    {
        //        debug_ct++;
        //        //Debug.Log(debug_ct);
        //        Debug.Log("left");
        //        timer = 0.0f;
        //        if (selected == options)
        //        {
        //            Highlight(play);
        //            selected = play;
        //        }
        //        else if (selected == controls)
        //        {
        //            Highlight(quit);
        //            selected = quit;
        //        }

        //        //horizIndex--;
        //        //vertIndex -= 2;
        //        //if (horizIndex < 0)
        //        //    horizIndex = 3;
        //        //if (vertIndex < 0)
        //        //    vertIndex = 3;
        //        //highlight button at horizCycle[horixIndex]
        //        //horizCycle[horizIndex].Select();
        //        //Highlight(horizCycle[horizIndex]);
        //        //selected = horizCycle[horizIndex];
        //    }
        //}

        //cycle up
        //else if (Input.GetAxisRaw("Vertical") > 0.5f)
        //{
        //    if (timer >= CycleDelay)
        //    {
        //        debug_ct++;
        //        //Debug.Log(debug_ct);
        //        Debug.Log("up");
        //        timer = 0.0f;
        //        if (selected == quit)
        //        {
        //            Highlight(play);
        //            selected = play;
        //        }
        //        else if (selected == controls)
        //        {
        //            Highlight(options);
        //            selected = options;
        //        }

        //        //vertIndex--;
        //        //horizIndex -= 2;
        //        //if (vertIndex < 0)
        //        //    vertIndex = 3;
        //        //if (horizIndex < 0)
        //        //    horizIndex = 3;
        //        //highlight button at vertCycle[vertIndex]
        //        //vertCycle[vertIndex].Select();
        //        //Highlight(vertCycle[vertIndex]);
        //        //selected = vertCycle[vertIndex];
        //    }
        //}

        //cycle down
        //if (Input.GetAxis("Vertical") < 0f  && Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(Input.GetAxis("Vertical")))
        //else if (Input.GetAxisRaw("Vertical") < -0.5f)
        //{
        //    if (timer >= CycleDelay)
        //    {
        //        debug_ct++;
        //        //Debug.Log(debug_ct);
        //        Debug.Log("down");
        //        timer = 0.0f;
        //        if (selected == play)
        //        {
        //            Highlight(quit);
        //            selected = quit;
        //        }
        //        else if (selected == options)
        //        {
        //            Highlight(controls);
        //            selected = controls;
        //        }

        //        //vertIndex++;
        //        //horizIndex += 2;
        //        //if (vertIndex > 3)
        //        //    vertIndex = 0;
        //        //if (horizIndex > 3)
        //        //    horizIndex = 0;
        //        //highlight button at vertCycle[vertIndex]
        //        //vertCycle[vertIndex].Select();
        //        //Highlight(vertCycle[vertIndex]);
        //        //selected = vertCycle[vertIndex];
        //    }
        //}
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Click"))
        {
            selected.onClick.Invoke();
        }
    }
}

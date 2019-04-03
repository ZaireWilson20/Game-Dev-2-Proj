using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverButtons : MonoBehaviour
{
    //Button[] vertCycle = new Button[4];
    Button[] vertCycle = new Button[2];
    Button tryAgain, titleScreen, selected;
    int vertIndex = 0;
    int debug_ct = 0;
    private String lastScene;

    public float CycleDelay = 0.0f;     //time between button cycling
    private float timer = 0.0f;

    public Color standard;
    public Color highlighted;

    //Start is called before the first frame update
    void Start()
    {
        //collect the buttons
        tryAgain = GameObject.Find("TryAgainButton").GetComponent<Button>();
        tryAgain.GetComponent<Image>().color = highlighted;
        titleScreen = GameObject.Find("TitleButton").GetComponent<Button>();
        titleScreen.GetComponent<Image>().color = standard;
        //options = GameObject.Find("OptionsButton").GetComponent<Button>();
        //options.GetComponent<Image>().color = standard;
        //controls = GameObject.Find("ControlsButton").GetComponent<Button>();
        //controls.GetComponent<Image>().color = standard;
        selected = tryAgain;

        //compile vertical ordering of buttons
        vertCycle[0] = tryAgain;
        vertCycle[1] = titleScreen;

        //get details of last scene
        lastScene = GlobalControl.Instance.savedScene.lastScene;

        //compile horizontal ordering of buttons
        //horizCycle[0] = play;
        //horizCycle[1] = options;
        //horizCycle[2] = quit;
        //horizCycle[3] = controls;

    }

    public void ReloadFightScene()
    {
        Debug.Log("reload last scene");
        SceneManager.LoadScene(lastScene, LoadSceneMode.Single);
    }

    public void LoadTitleScreen()
    {
        Debug.Log("load title screen");
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    void Highlight(Button b)
    {
        foreach (Button btn in vertCycle)
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
                if (selected == tryAgain)
                {
                    Highlight(titleScreen);
                    selected = titleScreen;
                }

                //horizIndex++;
                //if (horizIndex > 3)
                //    horizIndex = 0;
                //vertIndex += 2;
                //if (vertIndex > 3)
                //    vertIndex = 0;

                //Debug.Log(horizIndex);
                //horizCycle[horizIndex].Select();
                //highlight button at horizCycle[horixIndex]
                //Highlight(horizCycle[horizIndex]);
                //selected = horizCycle[horizIndex];
                //Debug.Log(selected);
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
                if (selected == titleScreen)
                {
                    Highlight(tryAgain);
                    selected = tryAgain;
                }

                //horizIndex--;
                //vertIndex -= 2;
                //if (horizIndex < 0)
                //    horizIndex = 3;
                //if (vertIndex < 0)
                //    vertIndex = 3;
                //highlight button at horizCycle[horixIndex]
                //horizCycle[horizIndex].Select();
                //Highlight(horizCycle[horizIndex]);
                //selected = horizCycle[horizIndex];
            }
        }
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Click"))
        {
            selected.onClick.Invoke();
        }
    }
}

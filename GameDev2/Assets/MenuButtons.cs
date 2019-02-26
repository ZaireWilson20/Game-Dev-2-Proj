using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
    //    LoadNewScene();
    //}

    public void LoadNextScene()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("ActualTechTutorial", LoadSceneMode.Single);
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
}

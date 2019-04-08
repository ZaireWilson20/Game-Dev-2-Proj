using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    //This is Main Camera in the scene
    Camera m_MainCamera;
    //This is the second Camera and is assigned in inspector
    public Camera m_CameraTwo;
    public GameObject playerObj;
    public GameObject cutsceneCam;

    void Start()
    {
    }

    void Update()
    {
        if (GlobalControl.Instance.savedScene.inCutScene)
        {
            playerObj.SetActive(false);
            cutsceneCam.SetActive(true);
        }
        else
        {
            playerObj.SetActive(true);
            cutsceneCam.SetActive(false);
        }
    }

}

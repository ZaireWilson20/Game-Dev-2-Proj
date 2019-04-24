using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Close()
    {
        SceneManager.LoadScene("TitlescreenNew", LoadSceneMode.Single);
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

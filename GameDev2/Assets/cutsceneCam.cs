using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneCam : MonoBehaviour
{
    public GameObject faderObj;
    public LevelSwitch levelSwitch; 

    // Start is called before the first frame update
    void Start()
    {
        levelSwitch = faderObj.GetComponent<LevelSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAnimFinish()
    {
        levelSwitch.FadeToLevel("Hub");
    }
}

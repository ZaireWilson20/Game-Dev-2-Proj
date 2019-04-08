using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public Animator anim;
    private string level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FadeToLevel(string levelName)
    {
        anim.SetTrigger("FadeOut");
        level = levelName;
    }

    public void LoadOnFadeComplete()
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

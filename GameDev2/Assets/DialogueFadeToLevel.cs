using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueFadeToLevel : MonoBehaviour
{
    private NpcDialogue npcDialogue;
    public string levelToFadeTo;
    public LevelSwitch levelSwitch;
    private float timer = 0.0f;
    public bool endGame = false;
    public float switchTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        npcDialogue = GetComponent<NpcDialogue>(); 
       
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame && timer > switchTime)
        {
            levelSwitch.FadeToLevel("Credits");
        }
        if (!endGame && npcDialogue.hasBeenRead)
        {
            levelSwitch.FadeToLevel(levelToFadeTo);
        }
        timer += Time.deltaTime;
    }
}

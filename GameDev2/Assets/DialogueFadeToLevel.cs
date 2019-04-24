using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueFadeToLevel : MonoBehaviour
{
    private NpcDialogue npcDialogue;
    public string levelToFadeTo;
    public LevelSwitch levelSwitch; 

    // Start is called before the first frame update
    void Start()
    {
        npcDialogue = GetComponent<NpcDialogue>(); 
       
    }

    // Update is called once per frame
    void Update()
    {
        if (npcDialogue.hasBeenRead)
        {
            levelSwitch.FadeToLevel(levelToFadeTo);
        }
    }
}

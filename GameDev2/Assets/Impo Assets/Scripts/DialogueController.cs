using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class DialogueController : MonoBehaviour
{
    int currentLine = 0;
    int totalLines; 
    private bool talking;
    public GameObject textBox;
    private Text dialogue;
    string[] allLines; 
    // Start is called before the first frame update
    void Start()
    {
        dialogue = textBox.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool IsTalking()
    {
        return talking; 
    }

    public void DisplayText(string[] lines)
    {
        textBox.SetActive(true);
        allLines = lines;
        totalLines = lines.Length;
        dialogue.text = lines[currentLine];
        currentLine++;
    }

    public bool nextLine()
    {
        if(currentLine == totalLines)
        {
            textBox.SetActive(false);
            currentLine = 0; 
            return false; 
        }
        dialogue.text = allLines[currentLine];
        currentLine++;
        return true; 
    }
}

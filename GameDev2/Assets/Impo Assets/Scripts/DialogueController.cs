using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor; 
using TMPro;
public class DialogueController : MonoBehaviour
{
    int currentLine = 0;
    int totalLines; 

    private bool talking;
    public GameObject textBox;
    public GameObject nameBox;
    public GameObject bigBox;
    public GameObject popUpBox; 
    public GameObject continueBox; 
    private TMP_Text dialogue;
    private TMP_Text nameText;
    private TMP_Text popText;
    private DialogueObj diaObj; 
    string[] allLines;
    public Image char1;
    public Image char2;
    private string path = "Assets/Sprites/Portraits/";
    private string fileName = ".png";
    private string firstSpeaker;
    private string secondSpeaker;
    public bool doneSentence = true; 
    // Start is called before the first frame update
    void Start()
    {
        dialogue = textBox.GetComponentInChildren<TMP_Text>();
        nameText = nameBox.GetComponentInChildren<TMP_Text>();
        popText = popUpBox.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool IsTalking()
    {
        return talking; 
    }

    public void DisplayText(DialogueObj lines)
    {
        diaObj = lines; 
        char1.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path + lines.fsSprite + fileName, typeof(Sprite)); 
        char2.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path + lines.ssSprite + fileName, typeof(Sprite));
        firstSpeaker = lines.firstSpeaker;
        secondSpeaker = lines.secondSpeaker;

        continueBox.SetActive(false);
        bigBox.SetActive(true);
        allLines = lines.allDialogue;
        totalLines = lines.allDialogue.Length;
        StartCoroutine(TypeSentence(allLines[currentLine]));
        setSpeaker(lines.speakerSeq[currentLine]);
        currentLine++;
    }

    public void DisplayPopUP(DialogueObj lines)
    {
        popUpBox.SetActive(true);
        popText.text = lines.allDialogue[0];
    }

    public void HidePopUp()
    {
        popUpBox.SetActive(false);
    }

    public bool nextLine()
    {
        if(currentLine == totalLines)
        {
            bigBox.SetActive(false);
            currentLine = 0; 
            return false; 
        }
        continueBox.SetActive(false);
        setSpeaker(diaObj.speakerSeq[currentLine]);
        StartCoroutine(TypeSentence(allLines[currentLine]));
        currentLine++;
        return true; 
    }
    
    private void setSpeaker(string speaker)
    {
        if (speaker == "fSpeaker")
        {
            nameText.text = firstSpeaker;
            char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, 1f);
            char2.color = new Color(char2.color.r, char2.color.g, char2.color.b, .5f);
        }
        else if (speaker == "sSpeaker")
        {
            nameText.text = secondSpeaker;
            char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, .5f);
            char2.color = new Color(char2.color.r, char2.color.g, char2.color.b, 1f);
        }
    }

    IEnumerator TypeSentence(string sent)
    {
        doneSentence = false;
        dialogue.text = "";
        foreach(char letter in sent)
        {
            dialogue.text += letter;
            yield return null; 
        }
        doneSentence = true;
        continueBox.SetActive(true);
    }
}

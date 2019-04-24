using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor; 
using TMPro;
public class DialogueController : MonoBehaviour
{
    public int currentLine = 0;
    int totalLines; 

    private bool talking;
    private Coroutine _coroutine;
    public GameObject dialogueBox;
    public GameObject nameBox;
    public GameObject conversationUI;   // gameobject as a whole which contains all the different chat elements
    public GameObject popUpBox; 
    public GameObject continueBox; 
    private TMP_Text dialogue;
    private TMP_Text nameText;
    private TMP_Text popText;
    private DialogueObj diaObj;
    private int curConvo = 0; 
    string[] allLines;
    public Image char1;
    public Image char2;
    private string path1 = "Assets/Sprites/Portraits/";
    //private string path2 = "Assets/Resources";
    private string fileName = ".png";
    private string firstSpeakerName;   
    private string secondSpeakerName;
    public bool doneSentence = false;
    public bool inCutscene = false; 
    // Start is called before the first frame update
    void Start()
    {
        dialogue = dialogueBox.GetComponentInChildren<TMP_Text>();
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

        //  Set Dialogue sprites for both characters in scene
#if UNITY_EDITOR
        char1.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + lines.fsSprite + "Neutral" + fileName, typeof(Sprite));
        char2.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + lines.ssSprite + "Neutral" + fileName, typeof(Sprite));

#else
        char1.sprite = Resources.Load<Sprite>(lines.fsSprite + lines.mood[currentLine]);
        char2.sprite = Resources.Load<Sprite>(lines.ssSprite + lines.mood[currentLine]);
#endif
        //char1.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path + lines.fsSprite + lines.mood[currentLine] + fileName, typeof(Sprite)); 
        //char2.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path + lines.ssSprite + lines.mood[currentLine] + fileName, typeof(Sprite));
        firstSpeakerName = lines.firstSpeaker;
        secondSpeakerName = lines.secondSpeaker;
        
        continueBox.SetActive(false);
        conversationUI.SetActive(true);
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
        if (gameObject.name == "HubInit" && currentLine == 5)
        {
            //inCutscene = true;
            //return true; 
        }
        // If speaker's last line
        if (currentLine == totalLines)
        {
            conversationUI.SetActive(false);
            currentLine = 0; 
            return false; 
        }

       
        continueBox.SetActive(false);

        setSpeaker(diaObj.speakerSeq[currentLine]);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(TypeSentence(allLines[currentLine]));
        currentLine++;
        return true; 
    }
    
    private void setSpeaker(string speaker)
    {

        //Debug.Log("Current Speaker: " + speaker);

        if (speaker == firstSpeakerName)
        {
            nameText.text = firstSpeakerName;
#if UNITY_EDITOR
            char1.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + diaObj.fsSprite + diaObj.mood[currentLine] + fileName, typeof(Sprite));
#else
            char1.sprite = Resources.Load<Sprite>(diaObj.fsSprite + diaObj.mood[currentLine]);
#endif
            //            char1.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + diaObj.fsSprite + diaObj.mood[currentLine] + fileName, typeof(Sprite));

            if(char1.sprite == null)
            {
                char1.gameObject.SetActive(false);
            }
            else
            {
                char1.gameObject.SetActive(true);
            }
            char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, 1f);
            char2.color = new Color(char2.color.r, char2.color.g, char2.color.b, .5f);

           
        }
        else if (speaker == secondSpeakerName)
        {
            nameText.text = secondSpeakerName;
#if UNITY_EDITOR
            char2.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + diaObj.ssSprite + diaObj.mood[currentLine] + fileName, typeof(Sprite));
#else
            char2.sprite = Resources.Load<Sprite>(diaObj.ssSprite + diaObj.mood[currentLine]);
#endif
            //            char2.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path1 + diaObj.ssSprite + diaObj.mood[currentLine] + fileName, typeof(Sprite));
            if (char2.sprite == null)
            {
                char2.gameObject.SetActive(false);
            }
            else
            {
                char2.gameObject.SetActive(true);
            }
            char1.color = new Color(char1.color.r, char1.color.g, char1.color.b, .5f);
            char2.color = new Color(char2.color.r, char2.color.g, char2.color.b, 1f);
        }
        else
        {
            //Debug.Log("Not Working");
        }
    }

    IEnumerator TypeSentence(string sent)
    {
        dialogue.text = "";
        int countBeforeSkip = 0; 
        foreach(char letter in sent)
        {
            if (Input.GetButtonDown("Pickup") && !doneSentence && countBeforeSkip > 2)
            {
                //Debug.Log("hot dog");
                break; 
            }
            dialogue.text += letter;
            yield return new WaitForSeconds(0f);
            countBeforeSkip++; 
        }
        dialogue.text = sent; 
        doneSentence = true;
        continueBox.SetActive(true);
        //Debug.Log("done sentence");
    }
}

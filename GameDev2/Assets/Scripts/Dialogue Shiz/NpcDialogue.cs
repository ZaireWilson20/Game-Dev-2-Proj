using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueController))]

public class NpcDialogue : MonoBehaviour
{

    private DialogueController npc_dialogue_cont;
    public DialogueData localData = new DialogueData(); 
    // Dialogue Container
    [SerializeField]
    private DialogueObj npc_convos;
    [SerializeField]
    private GameObject pa_game_manager;
    private NPC_Dialogue_Loader pa_dialogue;
    private GameState gameState;
    private DialogueObj diaData;

    public GameObject levelFader;
    private LevelSwitch levelSwitch;

    [SerializeField]
    GameObject npc_triggerObj;
    Dialogue_Trigger npc_onTrigger;

    [SerializeField]
    GameObject pa_playerObj;
    Player pa_script; 
    bool npc_manager_done;
    public bool npc_inConvo = false;
    DialogueController dialogueController;
    public bool automatedConvo;
    private bool finReading = true;
    private bool finishAuto = false;
    public bool popUpConvo;
    public bool byeGuy;
    public bool hasBeenRead;
    bool sceneLoaded = false; 
    private bool nextConvo = true; 
    private int convoNumber = 1;
    public int numOfConvos = 1; 


    public void CopySaved(NpcDialogue other)
    {
        npc_dialogue_cont = other.npc_dialogue_cont;
        npc_convos = other.npc_convos;
        pa_game_manager = other.pa_game_manager;
        pa_dialogue = other.pa_dialogue;
        gameState = other.gameState;
        diaData = other.diaData;
        levelFader = other.levelFader;
        levelSwitch = other.levelSwitch;
        npc_triggerObj = other.npc_triggerObj;
        npc_onTrigger = other.npc_onTrigger;
        pa_playerObj = other.pa_playerObj;
        pa_script = other.pa_script;
        npc_manager_done = other.npc_manager_done;
        npc_inConvo = other.npc_inConvo;
        dialogueController = other.dialogueController;
        automatedConvo = other.automatedConvo;
        finReading = other.finReading;
        finishAuto = other.finishAuto;
        popUpConvo = other.popUpConvo;
        byeGuy = other.byeGuy;
        hasBeenRead = other.hasBeenRead;
        sceneLoaded = other.sceneLoaded;
        nextConvo = other.nextConvo;
        convoNumber = other.convoNumber;
        numOfConvos = other.numOfConvos;
    }




    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("NPC STARTED");
        Debug.Log("GAME MANAGER IS: " + pa_game_manager.GetComponent<NPC_Dialogue_Loader>());

        localData = GlobalControl.Instance.savedDialogue;
        hasBeenRead = GlobalControl.Instance.savedDialogue.alreadyRead;
        dialogueController = GetComponent<DialogueController>();
        if (GlobalControl.Instance.savedScene.inCutScene)
        {
            GlobalControl.Instance.savedScene.inCutScene = false;
            //GlobalControl.Instance.savedScene.currentConversation = null;
            dialogueController.currentLine = GlobalControl.Instance.savedScene.currentConversationNum;
        }
        pa_dialogue = pa_game_manager.GetComponent<NPC_Dialogue_Loader>();
        gameState = pa_game_manager.GetComponent<GameState>();
        if (!automatedConvo)
        {
            npc_onTrigger = npc_triggerObj.GetComponent<Dialogue_Trigger>();
        }
            //pa_playerObj = GameObject.FindGameObjectWithTag("Player");
        pa_script = pa_playerObj.GetComponent<Player>();
        levelSwitch = levelFader.GetComponent<LevelSwitch>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pa_dialogue.pa_finished_reading && finReading)
        {
            npc_convos = pa_dialogue.GetConversation(transform.name + convoNumber);
            
            finReading = false;
            nextConvo = false;
        }

        if (!popUpConvo && !automatedConvo && !finReading)
        {
            if (npc_onTrigger.dia_player_in && Input.GetButton("Jump") && !npc_inConvo)    // If player presses talk button while in range of npc
            {
                pa_script.pa_inConvo = true;
                npc_inConvo = true;
                npc_onTrigger.dia_inConvo = true;
                dialogueController.DisplayText(npc_convos);
            }
            else if (npc_onTrigger.dia_player_in && Input.GetButton("Jump") && npc_inConvo && dialogueController.doneSentence)
            {
                Debug.Log("In next line part");
                dialogueController.doneSentence = false; 
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                npc_onTrigger.dia_inConvo = inC;
            }
            else
            {
                Debug.Log("nothing is happening");
            }
        }
        else if(automatedConvo && !finishAuto && !finReading && !hasBeenRead)
        {
            
            if (dialogueController.inCutscene && !sceneLoaded)
            {
                GlobalControl.Instance.savedDialogue.currentLine = dialogueController.currentLine;
                GlobalControl.Instance.savedScene.inCutScene = true;
                GlobalControl.Instance.savedScene.currentConversationNum = dialogueController.currentLine;
                sceneLoaded = true;
                levelSwitch.FadeToLevel("ActualTechTutorial");
            }
            if (!npc_inConvo)
            {
                pa_script.pa_inConvo = true;
                npc_inConvo = true;
                dialogueController.DisplayText(npc_convos);
            }
            else if(Input.GetButtonDown("Jump") && npc_inConvo && dialogueController.doneSentence)
            {
                dialogueController.doneSentence = false;
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                //npc_onTrigger.dia_inConvo = inC;
                if (!npc_inConvo)
                {
                    finishAuto = true;
                    hasBeenRead = true;
                    localData.alreadyRead = hasBeenRead;
                    GlobalControl.Instance.savedDialogue = localData; 
                }
            }
        }
        else if (popUpConvo && !finReading) 
        {
            //Debug.Log(npc_onTrigger.dia_player_in);

            if (npc_onTrigger.dia_player_in)
            {
                //Debug.Log("in here");
                dialogueController.DisplayPopUP(npc_convos);
            }
            else if(byeGuy)
            {
                //Debug.Log("I'm out");
                dialogueController.HidePopUp();

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueController))]

public class NpcDialogue : MonoBehaviour
{

    private DialogueController npc_dialogue_cont;
    private DialogueData localData = new DialogueData(); 
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
    public string sceneName; 
    bool npc_manager_done;
    public bool npc_inConvo = false;
    DialogueController dialogueController;
    private SceneTrigger npcTrigger; 
    //public bool automatedConvo;
    private bool finReading = true;
    private bool finishAuto = false;
    //public bool popUpConvo;
    public bool byeGuy;
    //public bool triggeredConvo;
    public bool hasBeenRead;
    bool sceneLoaded = false; 
    private bool nextConvo = true; 
    private int convoNumber = 1;
    public int numOfConvos = 1;
    public diaType convoType;
    public bool playConvo = true;
    public bool startOn;
    private bool init;
    public bool destroyOnDone; 


    public enum diaType { automated, triggered, restriction, pop_up, other };

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
        //automatedConvo = other.automatedConvo;
        finReading = other.finReading;
        finishAuto = other.finishAuto;
        //popUpConvo = other.popUpConvo;
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

        dialogueController = GetComponent<DialogueController>();
        if (GlobalControl.Instance.savedDialogue.InList(sceneName))
        {
            
            GlobalControl.Instance.savedScene.inCutScene = false;
            //GlobalControl.Instance.savedScene.currentConversation = null;
            dialogueController.currentLine = GlobalControl.Instance.savedDialogue.findCurrentLine(sceneName);
            
            hasBeenRead = GlobalControl.Instance.savedDialogue.HasBeenRead(sceneName);
            playConvo = GlobalControl.Instance.savedDialogue.IsOn(sceneName);
            init = false; 
        }
        else
        {
            Debug.Log("scene name: " + sceneName);
            GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, false);
            hasBeenRead = false;
            init = true;
        }


        pa_dialogue = pa_game_manager.GetComponent<NPC_Dialogue_Loader>();
        gameState = pa_game_manager.GetComponent<GameState>();
        if (convoType == diaType.automated || convoType == diaType.pop_up)
        {
            npc_onTrigger = npc_triggerObj.GetComponent<Dialogue_Trigger>();
        }
            //pa_playerObj = GameObject.FindGameObjectWithTag("Player");
        pa_script = pa_playerObj.GetComponent<Player>();
        levelSwitch = levelFader.GetComponent<LevelSwitch>();
        if (init)
        {
            Debug.Log("shit aint workin");
            GlobalControl.Instance.savedDialogue.SetState(sceneName, startOn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playConvo = GlobalControl.Instance.savedDialogue.IsOn(sceneName);
 
        //gameObject.SetActive(playConvo);
        if (pa_dialogue.pa_finished_reading && finReading)
        {
            npc_convos = pa_dialogue.GetConversation(transform.name);
            
            finReading = false;
            nextConvo = false;
        }


        if (convoType == diaType.other && !finReading)
        {
            /*
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
                //Debug.Log(GlobalControl.Instance.savedScene.findTrigger(sceneName).Triggered());
                //Debug.Log("nothing is happening");
            }*/
        }
        else if (convoType == diaType.automated && !finishAuto && !finReading && !hasBeenRead && playConvo)
        {
            if (dialogueController.inCutscene && !sceneLoaded)
            {
                
                GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, hasBeenRead);
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
            else if (Input.GetButtonDown("Jump") && npc_inConvo && dialogueController.doneSentence)
            {
                dialogueController.doneSentence = false;
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                //npc_onTrigger.dia_inConvo = inC;
                if (!npc_inConvo)   //  Finish convo and save info
                {
                    finishAuto = true;
                    hasBeenRead = true;
                    Debug.Log(sceneName + " has been read");
                    GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, hasBeenRead);
                    //GlobalControl.Instance.savedDialogue = localData;
                }
            }
        }
        else if (convoType == diaType.triggered && GlobalControl.Instance.savedScene.findTrigger(sceneName) && !finReading && !hasBeenRead && playConvo)
        {
            if (dialogueController.inCutscene && !sceneLoaded)
            {
                GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, hasBeenRead);
                //GlobalControl.Instance.savedDialogue.currentLine = dialogueController.currentLine;
                GlobalControl.Instance.savedScene.inCutScene = true;
                GlobalControl.Instance.savedScene.currentConversationNum = dialogueController.currentLine;
                sceneLoaded = true;
                levelSwitch.FadeToLevel("ActualTechTutorial");
            }
            if (!npc_inConvo)   //Start Convo
            {
                pa_script.pa_inConvo = true;
                npc_inConvo = true;
                dialogueController.DisplayText(npc_convos);
            }
            else if (Input.GetButtonDown("Jump") && npc_inConvo && dialogueController.doneSentence) // Press Space to continue conversation
            {
                dialogueController.doneSentence = false;
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                //npc_onTrigger.dia_inConvo = inC;
                if (!npc_inConvo)
                {
                    if(gameObject.name == "Castle2")
                    {
                        Debug.Log("Level Trans");
                        levelSwitch.MoveCharacterInScene();
                    }

                    finishAuto = true;
                    hasBeenRead = true;
                    GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, hasBeenRead);
                    //GlobalControl.Instance.savedDialogue = localData;
                }
            }
        }
        else if (convoType == diaType.pop_up && !finReading && playConvo)
        {
           
            if (!npc_inConvo && Input.GetButtonDown("Jump") && npc_onTrigger.dia_player_in)   //Start Convo
            {

                Debug.Log("HOLA");
                pa_script.pa_inConvo = true;
                npc_inConvo = true;
                dialogueController.DisplayText(npc_convos);
            }
            else if (npc_onTrigger.dia_player_in && Input.GetButtonDown("Jump") && npc_inConvo && dialogueController.doneSentence) // Press Space to continue conversation
            {
                dialogueController.doneSentence = false;
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                //npc_onTrigger.dia_inConvo = inC;
                if (!npc_inConvo)
                {
                    finishAuto = true;
                    //hasBeenRead = true;
                    GlobalControl.Instance.savedDialogue.SaveDialogue(sceneName, dialogueController.currentLine, hasBeenRead);
                    //GlobalControl.Instance.savedDialogue = localData;
                }
            }
        }
        else
        {
        }
        if (hasBeenRead && destroyOnDone)
        {
            DestroyOnConvoDone();
        }
    }

    public void DestroyOnConvoDone()
    {
        Destroy(this.gameObject);
    }
}

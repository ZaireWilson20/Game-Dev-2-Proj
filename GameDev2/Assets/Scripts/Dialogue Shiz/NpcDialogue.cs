using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private DialogueObj diaData;

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
    private bool nextConvo = true; 
    private int convoNumber = 1;
    public int numOfConvos = 1; 
    // Start is called before the first frame update
    void Start()
    {
        localData = GlobalControl.Instance.savedDialogue;
        hasBeenRead = GlobalControl.Instance.savedDialogue.alreadyRead;
        dialogueController = GetComponent<DialogueController>();
        pa_dialogue = pa_game_manager.GetComponent<NPC_Dialogue_Loader>();
        npc_onTrigger = npc_triggerObj.GetComponent<Dialogue_Trigger>();
        //pa_playerObj = GameObject.FindGameObjectWithTag("Player");
        pa_script = pa_playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pa_dialogue.pa_finished_reading && finReading)
        {
            npc_convos = pa_dialogue.GetConversation(transform.name + convoNumber);
            
            finReading = false;
            nextConvo = false;
            Debug.Log(finReading);
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
            if (!npc_inConvo)
            {
                pa_script.pa_inConvo = true;
                npc_inConvo = true;
                Debug.Log("Playing dia");
                dialogueController.DisplayText(npc_convos);
            }
            else if(Input.GetButtonDown("Jump") && npc_inConvo && dialogueController.doneSentence)
            {
                dialogueController.doneSentence = false;
                bool inC = dialogueController.nextLine();
                pa_script.pa_inConvo = inC;
                npc_inConvo = inC;
                npc_onTrigger.dia_inConvo = inC;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]

public class NpcDialogue : MonoBehaviour
{
    private DialogueController npc_dialogue_cont;

    // Dialogue Container
    [SerializeField]
    private string[] npc_convos;
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
    // Start is called before the first frame update
    void Start()
    {
        dialogueController = GetComponent<DialogueController>();
        pa_dialogue = pa_game_manager.GetComponent<NPC_Dialogue_Loader>();
        npc_onTrigger = npc_triggerObj.GetComponent<Dialogue_Trigger>();
        //pa_playerObj = GameObject.FindGameObjectWithTag("Player");
        pa_script = pa_playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pa_dialogue.pa_finished_reading)
        {
            npc_convos = pa_dialogue.GetConversation(transform.name);
        }


        if (npc_onTrigger.dia_player_in && Input.GetKeyDown(KeyCode.RightShift) && !npc_inConvo)    // If player presses talk button while in range of npc
        {
            Debug.Log("hi");
            pa_script.pa_inConvo = true;
            npc_inConvo = true;
            npc_onTrigger.dia_inConvo = true;
            dialogueController.DisplayText(npc_convos);
        }
        else if(npc_onTrigger.dia_player_in && Input.GetKeyDown(KeyCode.RightShift) && npc_inConvo)
        {
            Debug.Log("Gosh");
            bool inC = dialogueController.nextLine();
            pa_script.pa_inConvo = inC;
            npc_inConvo = inC;
            npc_onTrigger.dia_inConvo = inC;
        }        

    }
}

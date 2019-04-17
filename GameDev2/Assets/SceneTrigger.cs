using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    private bool sceneTrigger;
    public string _name;
    //public GameObject trigObject; 
    public bool finishedScene = false; 
    public enum requirements { door, sceneCollision , other };
    public requirements trigType;
    public GameObject doorObj;
    private DoorController doorController;
    private Player playerScript; 
    public bool turnsOffScenes;
    public bool turnOnScenes;
    private bool playerCollided;
    public GameObject[] convosInSceneToActivate; 
    

    [System.Serializable]
    public class ConversationTrigger
    {
        public string sceneName;
        //ublic bool isOn;
    }
    public ConversationTrigger turnOffConvo;
    public ConversationTrigger[] convosToActivate;
    public GameObject npcObj;
    private NpcDialogue npc; 
    // Start is called before the first frame update
    void Start()
    {
        if (trigType == requirements.door)
        {
            doorController = doorObj.GetComponent<DoorController>();
        }
        else
        {
            npc = npcObj.GetComponent<NpcDialogue>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (trigType == requirements.door && doorController.opened)
        {
            TriggerScene();
            if (turnsOffScenes)
            {
                GlobalControl.Instance.savedDialogue.TurnOff(turnOffConvo.sceneName);
            }
        }
        else if(trigType == requirements.sceneCollision && playerCollided)
        {
            //playerScript.anim.SetBool("Idle", true);
            TriggerScene();
            if (turnsOffScenes)
            {
                GlobalControl.Instance.savedDialogue.TurnOff(turnOffConvo.sceneName);
            }
            if (turnOnScenes)
            {
                foreach(GameObject c in convosInSceneToActivate)
                {
                    
                    c.SetActive(true);
                }
                foreach(ConversationTrigger c in convosToActivate)
                {
                    Debug.Log("setting " + c.sceneName + " to true");
                    GlobalControl.Instance.savedDialogue.SetState(c.sceneName, true);
                }
            }
        }
        else if(trigType == requirements.other && npc.hasBeenRead)
        {
            foreach (GameObject c in convosInSceneToActivate)
            {

                c.SetActive(true);
            }
            foreach (ConversationTrigger c in convosToActivate)
            {
                Debug.Log("setting " + c.sceneName + " to true");
                GlobalControl.Instance.savedDialogue.SetState(c.sceneName, true);
            }
        }
    }

    public void TriggerScene()
    {
        if (!sceneTrigger) {
            sceneTrigger = true;
            GlobalControl.Instance.savedScene.AddTrigger(this);
        }
    }

    public bool Triggered()
    {
        return sceneTrigger;
    }

    public void setTrigger()
    {
        sceneTrigger = true; 
    }

    public void breakTrigger()
    {
        sceneTrigger = false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerCollided = true;
            playerScript = collision.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCollided = false;
        }
    }


}

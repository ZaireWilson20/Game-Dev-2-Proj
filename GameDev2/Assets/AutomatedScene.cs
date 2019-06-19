using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AutomatedScene : MonoBehaviour
{
    public GameObject[] charactersInScene;

    private Player paraObj;
    public GameObject transformToWalkPara; 
    public GameObject gameManager;
    private GameState gameState;
    public GameObject portalGoingThrough; 
    private DoorController portalOpen; 
    public NpcDialogue sceneToWaitOn; 
    public bool beginScene; 
    int count = 0;
    public bool destroyOnDone; 
    //public delegate void Tasks();

    //public UnityEvent[] allFunct;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject c in charactersInScene)
        {
            if(c.GetComponent<Player>() != null)
            {
                paraObj = c.GetComponent<Player>();
            }
        }
        gameState = gameManager.GetComponent<GameState>();
        portalOpen = portalGoingThrough.GetComponent<DoorController>(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (sceneToWaitOn.hasBeenRead)
        {
            beginScene = true; 
        }
        if (beginScene)
        {
            switch (count)
            {
                case 0: //Move Paracelsys
                    gameState.cutscene = true;
                    bool reachedPos = paraObj.AutomatedWalkToPosition(transformToWalkPara.gameObject.transform.position);
                    if (reachedPos)
                    {
                        count++;
                    }
                    break;
                case 1:
                    Debug.Log("opening door");
                    portalOpen.OpenDoor();
                    count++;
                    break;
            }
        }
        if(destroyOnDone && count > 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void Sequencer(int sequenceIndex)
    {
        //if(sequenceIndex)
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnConvoStart : MonoBehaviour
{

    public GameObject mainDialogueObj;
    private NpcDialogue npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = mainDialogueObj.GetComponent<NpcDialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (npc.playConvo)
        {
            gameObject.SetActive(true);
        }
        else
        {
            //Debug.Log("Set to false");
            gameObject.SetActive(false);

        }
    }
}

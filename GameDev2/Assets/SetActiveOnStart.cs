using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnStart : MonoBehaviour
{
    public GameObject npcObj;
    private NpcDialogue npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = npcObj.GetComponent<NpcDialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(npc.playConvo);
    }
}

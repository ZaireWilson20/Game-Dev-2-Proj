using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue_Loader : MonoBehaviour
{
    Dictionary<string, DialogueObj > pa_npc_to_dia = new Dictionary<string, DialogueObj>(); 
    private GameObject[] pa_all_npcs;
    private NpcDialogue npcDia; 
    string path;
    string fileName = ".json";
    private DialogueObj diaData;
    public bool pa_finished_reading = false; 
    // Start is called before the first frame update
    void Start()
    {

        pa_all_npcs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject ga in pa_all_npcs)
        {
            //npcDia = ga.GetComponent<NpcDialogue>();
            for (int i = 1; i < 1 + 1; i++)
            {
                path = "Assets/Level Data/Dialogue Bank/" + ga.name + i + fileName;
                ReadData(path, ga.name + i);
            }
        }
        pa_finished_reading = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ReadData(string file_path, string name)
    {
        if (System.IO.File.Exists(file_path))
        {
            TextAsset targetFile = Resources.Load<TextAsset>(file_path);
            string content = System.IO.File.ReadAllText(file_path);
            diaData = JsonUtility.FromJson<DialogueObj>(content);
            pa_npc_to_dia.Add(name, diaData);
        }
        else
        {
            Debug.Log("Can't read file data");
        }
    }

    public DialogueObj GetConversation(string name)
    {
        return pa_npc_to_dia[name];
    }

}

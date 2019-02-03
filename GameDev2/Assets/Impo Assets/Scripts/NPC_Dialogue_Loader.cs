using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue_Loader : MonoBehaviour
{
    Dictionary<string, string[] > pa_npc_to_dia = new Dictionary<string, string[]>(); 
    private GameObject[] pa_all_npcs;
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
            path = Application.persistentDataPath + "/" + ga.name + fileName;
            ReadData(path, ga.name);
        }
        pa_finished_reading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (KeyValuePair<string, string[]> entry in pa_npc_to_dia)
            {
                Debug.Log("Name: " + entry.Key + " Dialogue: ");
                foreach(string g in entry.Value)
                {
                    Debug.Log(g);
                }
            }
        }
    }

    void ReadData(string file_path, string name)
    {
        if (System.IO.File.Exists(file_path))
        {
            string content = System.IO.File.ReadAllText(file_path);
            diaData = JsonUtility.FromJson<DialogueObj>(content);
            pa_npc_to_dia.Add(name, diaData.allDialogue);
        }
        else
        {
            Debug.Log("Can't read file data");
        }
    }

    public string [] GetConversation(string name)
    {
        return pa_npc_to_dia[name];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue_Loader : MonoBehaviour
{
    Dictionary<string, DialogueObj > pa_npc_to_dia = new Dictionary<string, DialogueObj>(); 
    public GameObject[] pa_all_npcs;
    private NpcDialogue npcDia; 
    string path;
    string fileName = ".json";
    private DialogueObj diaData;
    public bool pa_finished_reading = false;
    public string[] convosInScene; 
    // Start is called before the first frame update
    void Start()
    {

        //pa_all_npcs = GameObject.FindGameObjectsWithTag("NPC");


            //npcDia = ga.GetComponent<NpcDialogue>();
            foreach (string s in convosInScene)
            {
#if UNITY_EDITOR
            path = "Level Data/Dialogue Bank/" + s;
            //string path2 = "Level Data/Dialogue Bank/" + s;
            //TextAsset tarFile = Resources.Load<TextAsset>(path2);
            
            //DialogueObj tempData = JsonUtility.FromJson<DialogueObj>(tarFile.text);

            //Debug.Log(tempData.fsSprite);
#else
            path = "Level Data/Dialogue Bank/" + s; 
#endif


            ReadData(path, s);
            }
        int count = 0; 

        pa_finished_reading = true;

    }

    // Update is called once per frame
    void Update()
    {
    }

    
    void ReadData(string file_path, string name)
    {
        ///if (System.IO.File.Exists(file_path))
        //{
            TextAsset targetFile = Resources.Load<TextAsset>(file_path);
            //string content = System.IO.File.ReadAllText(file_path);
            diaData = JsonUtility.FromJson<DialogueObj>(targetFile.text);
            pa_npc_to_dia.Add(name, diaData);
        //}
        //else
        //{
            //Debug.Log("Can't read file data: " + file_path);
        //}
    }

    public DialogueObj GetConversation(string name)
    {
        return pa_npc_to_dia[name];
    }

}

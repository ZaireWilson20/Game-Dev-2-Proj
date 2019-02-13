using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post_Loader : MonoBehaviour
{
    Dictionary<string, string> pa_post_to_dia = new Dictionary<string, string>();
    private GameObject[] pa_all_npcs;
    string path;
    string fileName = ".json";
    private InfoObject infoData;
    // Start is called before the first frame update
    void Start()
    {

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
            infoData = JsonUtility.FromJson<InfoObject>(content);
            pa_post_to_dia.Add(name, infoData.infoText);
        }
        else
        {
            Debug.Log("Can't read file data");
        }
    }

    public string GetConversation(string name)
    {
        return pa_post_to_dia[name];
    }
}

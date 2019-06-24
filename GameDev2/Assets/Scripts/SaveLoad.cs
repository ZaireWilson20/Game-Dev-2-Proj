using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Class for saving our list of serialized global controls
public class SaveLoad
{
    public static List<GameData> savedGames = new List<GameData>();

    //Save function for saving a globalcontrol instance to our list of saved games
    //The binary formatter lets us convert our global control data to a file name of our choosing
    //savedGames.gd can be any filename and extension we want it to be
    //Even something like .fuck or .reallyLongExtensionName
    public static void Save()
    {
        savedGames.Add(GameData.activeGame);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<GameData>)bf.Deserialize(file);
            file.Close();
        }
    }
}
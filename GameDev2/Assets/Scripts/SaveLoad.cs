using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Class for saving our list of serialized global controls
public class SaveLoad
{
    public static List<GameData> savedGames = new List<GameData>();
    public static GameData activeGame;

    //Save function for saving a globalcontrol instance to our list of saved games
    //The binary formatter lets us convert our global control data to a file name of our choosing
    //savedGames.gd can be any filename and extension we want it to be
    //Even something like .fuck or .reallyLongExtensionName
    public static void Save()
    {
        if (activeGame == null)
        {
            activeGame = new GameData();
            Debug.Log("GameData should be set as new");
        }
        else
        {
            Debug.Log("GameData already init");
        }
        SaveGame();
        savedGames.Add(activeGame);
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

    public static void SaveGame()
    {
        activeGame.savedDialogue = GlobalControl.Instance.savedDialogue;
        activeGame.savedDoors = GlobalControl.Instance.savedDoors;
        activeGame.savedPickups = GlobalControl.Instance.savedPickups;
        activeGame.savedPlayer = GlobalControl.Instance.savedPlayer;
        //activeGame.savedScene = GlobalControl.Instance.savedScene;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    //player data
    public PlayerData savedPlayer = new PlayerData();
    public DialogueData savedDialogue = new DialogueData();
    public SceneData savedScene = new SceneData();
    public DoorData savedDoors = new DoorData();
    public PickupData savedPickups = new PickupData();
    
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            //GameData.activeGame = new GameData();
            //GameData.activeGame.savedPlayer = savedPlayer;
            //GameData.activeGame.savedScene = savedScene;
            //GameData.activeGame.savedDoors = savedDoors;
            //GameData.activeGame.savedDialogue = savedDialogue;
            //GameData.activeGame.savedPickups = savedPickups;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
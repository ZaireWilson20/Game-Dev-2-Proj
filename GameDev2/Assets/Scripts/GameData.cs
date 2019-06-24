using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameData
{
    public static GameData activeGame;

    public PlayerData savedPlayer = new PlayerData();
    public DialogueData savedDialogue = new DialogueData();
    public SceneData savedScene = new SceneData();
    public DoorData savedDoors = new DoorData();
    public PickupData savedPickups = new PickupData();

    public GameData()
    {
        savedPlayer = new PlayerData();
        savedDialogue = new DialogueData();
        savedScene = new SceneData();
        savedDoors = new DoorData();
        savedPickups = new PickupData();
    }
}

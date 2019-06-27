using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public float playerHealth = 5;
    public int playerHealthCap = 5; //absolute max health is 8
    public int points = 0;
    public bool canSwitch = false;
    public string posToSpawn = "CenterHub";
    public string levelName = "Hub";
    public Power mUtil;
    public Power tUtil;
    public Power mWeap;
    public Power tWeap;
    public Dictionary<string, Power> tUtils;
    public Dictionary<string, Power> mUtils;
    public Dictionary<string, Power> tWeaps;
    public Dictionary<string, Power> mWeaps;
    public bool reload = false;
    public bool factoryBossDefeated = false;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public float spawnX;
    public float spawnY;
    [System.NonSerialized]
    public Vector3 spawnPosition = new Vector3(8, 0, 0);
}

[System.Serializable]
public class DialogueData
{

    public bool alreadyRead = false;
    //public int currentLine = 0;
    public List<data> conversationData = new List<data>();

    [System.Serializable]
    public class data
    {
        public data(int curLine, string sceneName, bool hasBeenRead) {
            _name = sceneName;
            read = hasBeenRead;
            currentLine = curLine; 
        }
        public string _name; 
        public bool read;
        public int currentLine;
        public bool on = true; 
    }

    public bool HasBeenRead(string sceneName)
    {
        foreach(data i in conversationData)
        {
            if(i._name == sceneName)
            {
                return i.read; 
            }
        }
        return false; 
    }

    public int findCurrentLine(string sceneName)
    {
        foreach(data i in conversationData)
        {
            if(i._name == sceneName)
            {
                return i.currentLine;
            }
        }
        return -1; 
    }

    public void SaveDialogue(string sceneName, int curLine, bool _read)
    {
        foreach(data i in conversationData)
        {
            if(i._name == sceneName)
            {
                i.read = _read;
                i.currentLine = curLine; 
                return; 
            }
        }
        data tempData = new data(curLine, sceneName, _read);
        conversationData.Add(tempData);
    }

    public void FinReading(string sceneName)
    {
        for (int i = 0; i < conversationData.Count; i++)
        {
            if(conversationData[i]._name == sceneName)
            {
                conversationData[i].read = false; 
            }
        }
    }

    public bool InList(string sceneName)
    {
        for (int i = 0; i < conversationData.Count; i++)
        {
            if (conversationData[i]._name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool IsOn(string sceneName)
    {
        for (int i = 0; i < conversationData.Count; i++)
        {
            if (conversationData[i]._name == sceneName && conversationData[i].on)
            {
                return true; 
            }
        }
        return false;
    }
    public void TurnOff(string sceneName)
    {
        for (int i = 0; i < conversationData.Count; i++)
        {
            if (conversationData[i]._name == sceneName)
            {
                conversationData[i].on = false; 
            }
        }
    }

    public void SetState(string sceneName, bool state)
    {
        for (int i = 0; i < conversationData.Count; i++)
        {
            if (conversationData[i]._name == sceneName)
            {
                //Debug.Log("Set " + sceneName + " to " + state);
                conversationData[i].on = state;
                return;
            }
        }
        data tempData = new data(0, sceneName, false);
        conversationData.Add(tempData);
    }

}

[System.Serializable]
public class SceneData
{
    public String lastScene = "Hub";
    public bool inCutScene = false;
    public int currentConversationNum = 0;
    public bool playBoss = false; 
    [System.NonSerialized]
    public List<SceneTrigger> sceneTriggers = new List<SceneTrigger>();

    public void AddTrigger(SceneTrigger tempTrig)
    {
        SceneTrigger trigger = new SceneTrigger();
        trigger._name = tempTrig._name;
        if (tempTrig.Triggered())
        {
            trigger.setTrigger();
        }
        else
        {
            trigger.breakTrigger();
        }
        trigger.finishedScene = tempTrig.finishedScene;
        sceneTriggers.Add(trigger);
    }

    public bool findTrigger(string tName)
    {
        foreach (SceneTrigger i in sceneTriggers)
        {
            if (i._name == tName)
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class DoorData
{
    [System.Serializable]
    public class DoorsState
    {
        public bool unlocked;
        public string _name;
        public bool _active;
        public bool barrierOpened = false; 
    }
    public List<DoorsState> doors = new List<DoorsState>(); 
    public void SetDoorState(string doorName, bool state)
    {
        foreach(DoorsState d in doors)
        {
            if(d._name == doorName)
            {
                d.unlocked = state; 
            }
        }
    }
    public bool GetUnlocked(string doorName)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == doorName)
            {
                return d.unlocked; 
            }
        }
        return false; 
    }

    public bool InList(string doorName)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == doorName)
            {
                return true; 
            }
        }
        return false; 
    }

    public void AddDoor(string doorName, bool state, bool active)
    {
        DoorsState tempDoor = new DoorsState();
        tempDoor._name = doorName;
        tempDoor.unlocked = state;
        tempDoor._active = active;
        doors.Add(tempDoor);
    }

    public void SetPortalState(string portalName, bool state)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == portalName)
            {
                d._active = state;
            }
        }
    }

    public bool GetActive(string doorName)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == doorName)
            {
                return d._active;
            }
        }
        return false; 
    }

    public void SetBarrierOpen(string doorName)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == doorName)
            {
                d.barrierOpened = true;
            }
        }
    }

    public bool GetBarrierOpen(string doorName)
    {
        foreach (DoorsState d in doors)
        {
            if (d._name == doorName)
            {
                return d.barrierOpened;
            }
        }
        return false; 
    }
}

[System.Serializable]
public class PickupData
{
    public Hashtable pickupTable = new Hashtable();
    //public List<String> powersAndKeys = new List<String>();
    //public List<String> pointPickups = new List<String>();
    //public PowerSwitchPickup powerSwitch;

    public bool InTable(PickUp pickup)
    {
        return pickupTable.ContainsKey(pickup._name);
    }

    public bool InTable(PointPickup pickup)
    {
        return pickupTable.ContainsKey(pickup.id);
    }

    public bool InTable(PowerSwitchPickup pickup)
    {
        return pickupTable.ContainsKey("power switch");
    }
}
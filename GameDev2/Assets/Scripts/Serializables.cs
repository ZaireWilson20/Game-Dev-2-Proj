using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData
{
    public int playerHealth = 5;
    public Power mUtil;
    public Power tUtil;
    public Power mWeap;
    public Power tWeap;
    public Dictionary<string, Power> tUtils;
    public Dictionary<string, Power> mUtils;
    public Dictionary<string, Power> tWeaps;
    public Dictionary<string, Power> mWeaps;
    public bool reload = false;
}

public class DialogueData
{
    public bool alreadyRead = false; 
}

public class SceneData
{
    public String lastScene = "Hub";
}
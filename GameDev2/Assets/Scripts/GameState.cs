﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public bool paused = false;

    private void Update()
    {
        if (paused)
        {
            //Time.timeScale = 0f;
        }
    }
}
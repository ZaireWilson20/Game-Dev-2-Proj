﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public string music = "event:/CastleBackground";

    public FMOD.Studio.EventInstance musicEv; 
    // Start is called before the first frame update
    void Start()
    {

        //musicEv = FMODUnity.RuntimeManager.CreateInstance(music);
        //musicEv.start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSong(string song)
    {
        //musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicEv = FMODUnity.RuntimeManager.CreateInstance(song);
    }
}

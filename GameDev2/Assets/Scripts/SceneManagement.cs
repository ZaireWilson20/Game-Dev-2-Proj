﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public string levelName;
    GameObject player;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player>().SavePlayer();
            player.GetComponent<Player>().SaveSceneData();

            Debug.Log("Player should be saved");        
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

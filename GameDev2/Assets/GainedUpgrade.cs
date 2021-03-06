﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainedUpgrade : MonoBehaviour
{
    private float notifTimer = -1f;
    public TMP_Text text;

    public void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void newNotif(string name)
    {
        text.SetText(name);
        Debug.Log("Called new notif");
        notifTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (notifTimer >= 0)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
        notifTimer -= Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public GameObject[] healthSprites;
    private int currentLife = 4;

    private void Start()
    {
        while (currentLife > GlobalControl.Instance.savedPlayer.playerHealth - 1)
            loseHealth();
    }

    public void loseHealth()
    {
        healthSprites[currentLife].SetActive(false);
        currentLife--;
        if (currentLife < 0)
        {
            currentLife = 0;
        }
    }

    public void gainHealth()
    {
        if (currentLife < 4)
        {
            healthSprites[currentLife + 1].SetActive(true);
            currentLife++;
        }

    }
}

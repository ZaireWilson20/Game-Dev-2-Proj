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
        //should have five butterflies if 4 < health <= 5 
        while (GlobalControl.Instance.savedPlayer.playerHealth - currentLife <= 0)
            loseHealth(GlobalControl.Instance.savedPlayer.playerHealth);
    }

    public void loseHealth(float health)
    {
        if (health - currentLife <= 0)
        {
            healthSprites[currentLife].SetActive(false);
            currentLife--;
            if (currentLife < 0)
            {
                currentLife = 0;
            }
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

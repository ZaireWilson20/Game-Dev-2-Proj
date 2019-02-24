using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public GameObject[] healthSprites;
    private int currentLife = 4;

    public void loseHealth()
    {
        healthSprites[currentLife].SetActive(false);
        currentLife--;
    }


}

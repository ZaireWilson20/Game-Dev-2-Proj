using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public GameObject[] healthSprites;
    public Image background;
    private int currentLife;

    private void Start()
    {
        //should have five butterflies if 4 < health <= 5 
        currentLife = GlobalControl.Instance.savedPlayer.playerHealthCap - 1;
        for (int i = currentLife + 1; i < 8; i++)
        {
            healthSprites[i].SetActive(false);
            //background.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, background.rectTransform.rect.width - 100);
        }
        background.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, GlobalControl.Instance.savedPlayer.playerHealthCap * 100);

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
        if (currentLife < GlobalControl.Instance.savedPlayer.playerHealthCap)
        {
            healthSprites[currentLife + 1].SetActive(true);
            currentLife++;
        }
        background.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, GlobalControl.Instance.savedPlayer.playerHealthCap * 100);
    }
}

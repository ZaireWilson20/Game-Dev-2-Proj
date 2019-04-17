using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SalHealthBar : MonoBehaviour
{

    public GameObject boss;
    private SalBoss salscript;
    private float health_max;
    public Slider healthBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        salscript = boss.GetComponent<SalBoss>();
        health_max = salscript.maxHealth;
        //shieldSize = shieldZone.rectTransform.rect;
        //shieldWidth = shieldSize.xMax - shieldSize.xMin;
        //shield_xmax = shieldSize.xMax;
        //Debug.Log("xmax init: " + shield_xmax);
    }

    private float GetHealthLost(float health)
    {
        //amount of health lost, scaled from 0 to 1
        return (health_max - health) / health_max;
    }

    // Update is called once per frame
    void Update()
    {
        float cur_health = salscript.GetHealth();
        healthBarSlider.value = 1 - GetHealthLost(cur_health);
        if (salscript.GetHealth() <= 0)
        {
            healthBarSlider.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject boss;
    private FactoryBoss fscript;
    private bool factory = true;
    public Slider healthBarSlider;
    public Image shieldZone;
    private float health_max;
    private float shield_health;
    Image[] images;
    Image bar, bkgnd;

    // Start is called before the first frame update
    void Start()
    {
        if (boss.tag.Contains("Factory"))
        {
            factory = true;
            fscript = boss.GetComponent<FactoryBoss>();
            health_max = fscript.health_max;
            shield_health = fscript.shield_health;
        
        } else
        {
            factory = false;
        }
        //shieldSize = shieldZone.rectTransform.rect;
        //shieldWidth = shieldSize.xMax - shieldSize.xMin;
        //shield_xmax = shieldSize.xMax;
        //Debug.Log("xmax init: " + shield_xmax);
        images = GetComponentsInChildren<Image>();
        foreach (Image im in images)
        {
            if (im.name.Equals("Fill"))
                bar = im;
            else if (im.name.Equals("Background"))
                bkgnd = im;
        }
    }

    private float GetHealthLost(float health)
    {
       //amount of health lost, scaled from 0 to 1
       return (health_max - health) / health_max;
    }

    // Update is called once per frame
    void Update()
    {
        if (factory)
        {
            float cur_health = fscript.GetHealth();
            healthBarSlider.value = 1 - GetHealthLost(cur_health);
            if (cur_health <= shield_health)
            {
                bar.color = shieldZone.color;
                shieldZone.enabled = false;
                //                original value                scaled health lost from initial
            }
            if (healthBarSlider.value <= 0)
            {
                bar.enabled = false;
            }

        }
    }
}

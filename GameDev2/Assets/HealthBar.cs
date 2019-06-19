using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject boss;
    private FactoryBoss fscript;
    private SalBoss salscript;
    private bool factory = true;
    public Slider healthBarSlider;
    public Image shieldZone;
    private float health_max;
    private float shield_health;
    Image[] images;
    Image bar, bkgnd;
    Image[] bigShields = new Image[2];
    private int bigCt = 0;
    Image[] barShields = new Image[3];
    private int barCt = 0;

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
            else if (im.name.Contains("BarShield"))
            {
                barShields[barCt] = im;
                barCt++;
            } else if (im.name.Contains("BigShield")) {
                bigShields[bigCt] = im;
                im.enabled = false;
                bigCt++;
            }
        }
    }

    private float GetHealthLost(float health)
    {
       //amount of health lost, scaled from 0 to 1
       return (health_max - health) / health_max;
    }

    private void EnableShields(Image[] shields, bool enable)
    {
        foreach (Image im in shields)
            im.enabled = enable;
    }

    private void DisableShield(float health)
    {
        foreach (Image im in barShields)
        {
            if (im.name.Contains("Right") && health < 0.75 * shield_health)
                im.enabled = false;
            else if (im.name.Contains("Center") && health < 0.5 * shield_health)
                im.enabled = false;
            else if (im.name.Contains("Left") && health < 0.25 * shield_health)
                im.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (factory)
        {
            if (fscript.IsDead())
                this.gameObject.SetActive(false);
            float cur_health = fscript.GetHealth();
            healthBarSlider.value = 1 - GetHealthLost(cur_health);
            if (cur_health <= shield_health)
            {
                EnableShields(bigShields, true);
                DisableShield(cur_health);
                bar.color = shieldZone.color;
                shieldZone.enabled = false;
                //                original value                scaled health lost from initial
            }
            if (healthBarSlider.value <= 0)
            {
                bar.enabled = false;
                EnableShields(bigShields, false);
                EnableShields(barShields, false);
            }

        }
    }
}

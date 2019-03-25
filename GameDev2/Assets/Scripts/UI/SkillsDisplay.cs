using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsDisplay : MonoBehaviour
{
    public GameObject playerObj;
    Player playerScript;
    int sci_amount_active = 0;
    int mag_amount_active = 0;
    public GameObject[] magicPowers;
    public GameObject[] sciencePowers;
    int sciCount = 0;
    int magCount = 0; 
    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Count Science active powers
        foreach (KeyValuePair<string, Power> pow in playerScript.tPowerDict)     //Powers
        {
            if (pow.Value.active)
            {
                foreach (GameObject powImage in sciencePowers)
                {
                    if (powImage.name == pow.Key && !pow.Value.inList)
                    {
                        powImage.SetActive(true);
                        pow.Value.inList = true;
                    }
                }
                sci_amount_active++;
            }
        }

        foreach (KeyValuePair<string, Power> pow in playerScript.tWeaponDict)   //Weapons
        {

            if (pow.Value.active)
            {
                foreach (GameObject powImage in sciencePowers)
                {
                    if (powImage.name == pow.Key && !pow.Value.inList)
                    {
                        powImage.SetActive(true);
                        pow.Value.inList = true;
                    }
                }
                sci_amount_active++;
            }
        }

        //Count Magic active powers
        foreach (KeyValuePair<string, Power> pow in playerScript.mPowerDict)    //Powers
        {
            if (pow.Value.active)
            {
                foreach (GameObject powImage in magicPowers)
                {
                    if (powImage.name == pow.Key && !pow.Value.inList)
                    {
                        powImage.SetActive(true);
                        pow.Value.inList = true;
                    }
                }
                mag_amount_active++;
            }
        }

        foreach (KeyValuePair<string, Power> pow in playerScript.mWeaponDict)   //Weapons
        {
            if (pow.Value.active)
            {
                foreach (GameObject powImage in magicPowers)
                {
                    if (powImage.name == pow.Key && !pow.Value.inList)
                    {
                        powImage.SetActive(true);
                        pow.Value.inList = true;
                    }
                }
                mag_amount_active++;
            }
        }
    }
}

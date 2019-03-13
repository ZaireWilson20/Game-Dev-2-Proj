using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerSetController : MonoBehaviour
{
    public TMP_Text powerSetDisplay; 

    public void showPowerSet(string power)
    {
        powerSetDisplay.text = power; 
    }
}

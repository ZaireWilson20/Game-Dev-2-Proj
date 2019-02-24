using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerSetController : MonoBehaviour
{
    public Text powerSetDisplay; 

    public void showPowerSet(string power)
    {
        powerSetDisplay.text = power; 
    }
}

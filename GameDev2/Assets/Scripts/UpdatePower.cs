using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdatePower : MonoBehaviour
{
    private Player pscript;
    private string newText;
    public TMP_Text tmp;
    // Start is called before the first frame update
    void Start()
    {
        pscript = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        newText = "Power: " + pscript.points;
        tmp.SetText(newText);
    }
}

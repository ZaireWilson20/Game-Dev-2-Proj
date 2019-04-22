using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedsKeyNotification : MonoBehaviour
{
    /*
    public string _name = "";
    TMP_Text tmpText;
    private RectTransform rectTransform;
    private Vector2 uiOffset;
    Canvas gameCanvas; 
    // Start is called before the first frame update
    void Start()
    {
        // Get the rect transform
        this.rectTransform = GetComponent<RectTransform>();
        //gameCanvas.GetComponent<RectTransform>().sizeDelta.
        // Calculate the screen offset
        this.uiOffset = new Vector2((float)gameCanvas.GetComponent<RectTransform>().sizeDelta.x / 2f, (float)Canvas.sizeDelta.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(name != "")
        {
            tmpText.SetText("You Need The Spell " + _name + " To Open Barrier");
        }
    }

    /// <summary>
    /// Move the UI element to the world position
    /// </summary>
    /// <param name="objectTransformPosition"></param>
    public void MoveToClickPoint(Vector3 objectTransformPosition)
    {
        // Get the position on the canvas
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * Canvas.sizeDelta.x, ViewportPosition.y * Canvas.sizeDelta.y);

        // Set the position and remove the screen offset
        this.rectTransform.localPosition = proportionalPosition - uiOffset;
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool dia_player_in;

    [SerializeField]
    private GameObject dia_icon;
    public bool dia_inConvo;

    public GameObject diaObj;
    private NpcDialogue diaScript; 
    void Start()
    {
        dia_icon.SetActive(false);
        diaScript = diaObj.GetComponent<NpcDialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dia_player_in && !dia_inConvo)
        {
            //  Show pop up icon
            //Debug.Log("in");
            dia_icon.SetActive(true);
        }
        else
        {
            // Remover pop up icon
            dia_icon.SetActive(false);
        }

        //if(dia_inConvo)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            dia_player_in = true;
            diaScript.byeGuy = false; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Debug.Log("COME ON");
            diaScript.byeGuy = true; 
            dia_player_in = false;
        }
    }


    public void Deactivate()
    {
        dia_icon.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_Text : MonoBehaviour
{
    [SerializeField]
    private GameObject pa_gameManager;
    private Post_Loader pa_textLoader; 

    // Start is called before the first frame update
    void Start()
    {
        pa_textLoader = pa_gameManager.GetComponent<Post_Loader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

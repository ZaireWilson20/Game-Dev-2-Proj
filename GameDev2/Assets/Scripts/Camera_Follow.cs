using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField]
    private GameObject pa_playerRef;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            transform.position = new Vector3(pa_playerRef.transform.position.x, pa_playerRef.transform.position.y, pa_playerRef.transform.position.z - 10);
        
    }
}

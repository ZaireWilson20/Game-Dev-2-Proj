using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField]
    private GameObject pa_playerRef;

    [SerializeField]
    private GameObject pa_DeadZone;
    private Spawner pa_spawner; 
    // Start is called before the first frame update
    void Start()
    {
        pa_spawner = pa_DeadZone.GetComponent<Spawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pa_spawner.WaitingForSpawn())
        {
            transform.position = new Vector3(pa_spawner.GetPos().x, pa_spawner.GetPos().y, pa_spawner.GetPos().z - 10);
        }
        else
        {
            transform.position = new Vector3(pa_playerRef.transform.position.x, pa_playerRef.transform.position.y, pa_playerRef.transform.position.z - 10);
        }
    }
}

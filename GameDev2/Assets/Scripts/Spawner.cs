using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnPoint; //   Might implement as array later to handle ltiple checkpoints
    private Vector3 pointAtDeath;
    private bool waitingForSpawn;
    private GameObject collidedObject; 
    // Start is called before the first frame update
    void Start()
    {
        waitingForSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" )
        {
            collision.gameObject.GetComponent<Player>().alive = false; 
            collidedObject = collision.gameObject; 
            pointAtDeath = collidedObject.transform.position;
            waitingForSpawn = true; 
            collidedObject.SetActive(false);
            StartCoroutine(SpawnDelay());
            collidedObject.transform.position = spawnPoint.transform.position;
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1f);
        collidedObject.SetActive(true);

        if (collidedObject.tag == "Player") {
            collidedObject.gameObject.GetComponent<Player>().alive = true; 
        }
        waitingForSpawn = false; 
    }

    public bool WaitingForSpawn()
    {
        return waitingForSpawn; 
    }

    public Vector3 GetPos()
    {
        return pointAtDeath;
    }
}


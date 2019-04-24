using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            this.gameObject.GetComponent<Player>().spawnPosition = new Vector2(0, 0);
            SceneManager.LoadScene("BossTest");
        }
    }
}

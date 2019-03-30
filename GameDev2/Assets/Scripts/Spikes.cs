using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float descendSpeed = 1f;
    public float ascendSpeed = 3f;
    public GameObject lowObj;
    public GameObject highObj; 
    private Vector3 lowPoint;
    private Vector3 highPoint;
    private bool movingDown = true;
    private GameObject playerObj;
    public GameObject parent; 
    private Player player;
    public bool moving; 
    // Start is called before the first frame update
    void Start()
    {
        lowPoint = lowObj.transform.position;
        highPoint = highObj.transform.position;
        player = GameObject.Find("Paracelsys").GetComponent<Player>();
        //player = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        if (moving)
        {
            if (transform.position.y < lowPoint.y)
            {
                movingDown = false;
            }
            else if (transform.position.y > highPoint.y)
            {
                movingDown = true;
            }

            if (movingDown)
            {
                transform.Translate(0f, -1 * descendSpeed * Time.deltaTime, 0f);
            }
            else
            {
                transform.Translate(0f, ascendSpeed * Time.deltaTime, 0f);
            }
            Transform parentTrans = GetComponentInParent<Transform>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.takeDamage(1, new Vector3(Mathf.Sign(player.velocity.x) * -1f, 1f, 0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
    public Transform target;
    public float speed;
    public bool movingVertical; 
    private bool moveUp;
    private bool playerTouching;
    private int dir = 0;    //positive for down/right, negative for up/left 
    private Vector2 vel;

    private Vector3 prevPos; 
    private GameObject player; 
    private bool positive; 

    void Start()
    {
        startPos = transform.position;
        if (movingVertical)
        {
            if (startPos.y > target.position.y)
                dir = 1;    //move down
            else if (startPos.y < target.position.y)
                dir = -1;   //move up
        } else
        {
            if (startPos.x > target.position.x)
                dir = -1;   //move left
            else if (startPos.x < target.position.x)
                dir = 1;    //move right
        }
        //moveUp = true;
        //prevPos = transform.position;
    }
    void Update()
    {
        //float step = speed * Time.deltaTime;
        //Vector3 currPos = transform.position - prevPos;
        //prevPos = transform.position;
        //if(currPos.x < 0){
        //    positive = false;
        //}
        //else{
        //    positive = true;
        //}
        //if(playerTouching && !movingVertical){
        //   //player.transform.position = new Vector3(player.transform.position.x + (positive ? step : step*-1f), player.transform.position.y, player.transform.position.z);
        //}
        //if (transform.position == target.position)
        //{
        //    moveUp = false;
        //}
        //else if (transform.position == startPos)
        //{
        //    moveUp = true;
        //}
        //if(moveUp == false)
        //{
        //    transform.position = Vector3.MoveTowards (transform.position, startPos, step);
        //}
        //else if (moveUp)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        //}
        if (movingVertical)
        {
            if (transform.position.y >= target.position.y)
                dir = 1;    //move down
            else if (transform.position.y <= startPos.y)
                dir = -1;   //move up
            vel = Vector2.down * dir;
        }
        else
        {
            if (transform.position.x >= target.position.x)
                dir = -1;   //move left
            else if (transform.position.x <= startPos.x)
                dir = 1;    //move right
            vel = Vector2.right * dir;
        }
        GetComponent<Rigidbody2D>().velocity = speed * vel;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            playerTouching = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            playerTouching = false;
        }
    }
}

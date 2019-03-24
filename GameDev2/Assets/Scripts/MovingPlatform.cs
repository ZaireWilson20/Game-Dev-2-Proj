using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
     public Transform target;
     public float speed;
     private bool moveUp;
     private bool playerTouching; 

    private Vector3 prevPos; 
     private GameObject player; 
     private bool positive; 
     void Start()
     {
         startPos = transform.position;
         moveUp = true;
         prevPos = transform.position;
     }
     void Update()
     {
         float step = speed * Time.deltaTime;
         Vector3 currPos = transform.position - prevPos;
         prevPos = transform.position;
         if(currPos.x < 0){
             positive = false;
         }
         else{
             positive = true;
         }
         if(playerTouching){
             player.transform.position = new Vector3(player.transform.position.x + (positive ? step : step*-1f), player.transform.position.y, player.transform.position.z);
         }
         if (transform.position == target.position)
         {
             moveUp = false;
         }
         else if (transform.position == startPos)
         {
             moveUp = true;
         }
         if(moveUp == false)
         {
             transform.position = Vector3.MoveTowards (transform.position, startPos, step);
         }
         else if (moveUp)
         {
             transform.position = Vector3.MoveTowards(transform.position, target.position, step);
         }
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

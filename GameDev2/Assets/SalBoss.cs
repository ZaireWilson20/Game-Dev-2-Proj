﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalBoss : MonoBehaviour
{
    public int locations = 4;
    private int currentLocation = 1;
    public int attacks = 2;
    private int lastAttack = 0;
    public float maxHealth = 30f;
    private float health;
    public float phase2Health;

    public GameObject minion;
    public GameObject heatSeeking;
    public GameObject fireWall1;
    public GameObject fireWall2;
    public GameObject fireWall3;
    private GameObject newEnemy;
    private GameObject newProjectile;

    public float timeToAttack = 2f;
    public float timeToTeleport = 1f;
    private float maxTimeToTP;
    private float timeSinceLastAttack = 0;
    private float numTeleports = 0;
    private float timeSinceLastTeleport = 0;
    private float animTime = 0f;
    private float fireWallSummon = 1.5f;
    private float tpOut = 0;

    private bool flash = false;
    public int flashRate = 3;
    private int flashCt = 0;
    private bool invincible = false;
    private bool dead = false;

    GameObject player;
    Player pscript;

    private SpriteRenderer spr;
    public Animator anim;

    public NpcDialogue npcDialogueToTurnOn;
    public bool startBoss; 

    public float GetHealth()
    {
        return health;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        maxTimeToTP = timeToTeleport;
        //phase2Health = maxHealth / 2;
        spr = GetComponent<SpriteRenderer>();
        Random.InitState((int)(1000*Random.value));
    }

    public void takeDamage(float damage)
    {
        //anim.SetFloat("Health", health);

        if (!invincible)
        {
            //play hurt animation
            //anim.SetTrigger("Damaged");

            health -= damage;
            if (health <= phase2Health)
            {
                Debug.Log("Phase 2 Start");
                timeToTeleport = maxTimeToTP / 2;
                attacks = 3;
            }
            if (health <= 0)
            {
                //play death animation
                Debug.Log("Boss has died");
                anim.SetTrigger("Death");
                dead = true;
                animTime = .5f;
            }

            flash = true;
            spr.enabled = false;
            invincible = true;
            flashCt = 0;
            Debug.Log("Enemy health: " + health);
        }
    }

    //Randomly chooses a new location that is different from the last one
    int chooseLocation()
    {
        int newLocation = currentLocation;
        while (newLocation == currentLocation)
        {
            newLocation = (int)(3*Random.value)+1;
            //Debug.Log(newLocation);
        }
        return newLocation;
    }

    //Same as above but for attacks
    int chooseAttack()
    {
        int nextAttack = lastAttack;
        while (nextAttack == lastAttack)
        {
            nextAttack = (int)(attacks * Random.value) + 1;
            //Debug.Log(nextAttack);
        }
        return nextAttack;
    }

    Vector2 teleportToLocation(int Loc)
    {
        Debug.Log("teleporting");
        Vector2 Location = new Vector2();
        if (Loc == 1)
        {
            Location.x = 7.9f;
            Location.y = 4f;
        }
        else if (Loc == 2)
        {
            Location.x = 0;
            Location.y = 2.5f;
        }
        else if (Loc == 3)
        {
            Location.x = 15.8f;
            Location.y = 2.5f;
        }
        else
        {
            Location.x = 7.9f;
            Location.y = 8;
        }
        //anim.SetTrigger("TPIn");
        //animTime = tpOut;
        return Location;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GlobalControl.Instance.savedScene.playBoss)
        {
            transform.position = new Vector3(7.897f, -1.22f);
        }
        else
        {
            if (flash)
            {
                if (flashCt < flashRate)
                {
                    flashCt++;
                }
                else
                {
                    invincible = false;
                    flash = false;
                    spr.enabled = true;
                }
            }

            //currentLocation = chooseLocation();
            //lastAttack = chooseAttack();

            //Behavior process
            //Phase 1
            //  1. After some time since the last attack action, teleport
            //  2. After some time since the last teleport action, attack
            //Phase 2
            //  1. If teleport other than last happened, wait then teleport
            //  2. If last teleport happened, attack
            //  3. If attacked last, begin teleporting
            if (!dead)
            {
                if (timeSinceLastAttack < 0f)
                {
                    //Teleport when appropriate
                    if (timeSinceLastTeleport < 0f)
                    {
                        currentLocation = chooseLocation();
                        transform.position = teleportToLocation(currentLocation);
                        timeSinceLastTeleport = timeToTeleport;
                        numTeleports++;
                    }
                    //Phase 1 Attack/Teleport Pattern
                    if (numTeleports == 1 && phase2Health < health)
                    {
                        numTeleports = 0;
                        lastAttack = chooseAttack();
                        if (lastAttack == 1)
                        {
                            currentLocation = chooseLocation();
                            transform.position = teleportToLocation(currentLocation);
                            newProjectile = Instantiate(heatSeeking, transform.position, transform.rotation) as GameObject;
                            newProjectile.SetActive(true);
                            Debug.Log("Heat seeking");
                            anim.SetTrigger("Shoot");
                        }
                        if (lastAttack == 2)
                        {
                            currentLocation = 4;
                            transform.position = teleportToLocation(4);
                            Vector2 spawnPos = new Vector2();
                            if ((int)(Random.value * 2) == 1) spawnPos.x = -4;
                            else spawnPos.x = 20;
                            spawnPos.y = -1.4f;
                            newEnemy = Instantiate(minion, spawnPos, transform.rotation) as GameObject;
                            newEnemy.SetActive(true);
                            anim.SetTrigger("Summon");
                            Debug.Log("Summon guy");
                        }
                        timeSinceLastAttack = timeToAttack;
                    } //Phase 2 Attack/Teleport Pattern
                    else if (numTeleports == 3 && phase2Health >= health)
                    {
                        numTeleports = 0;
                        lastAttack = chooseAttack();
                        if (lastAttack == 1)
                        {
                            currentLocation = chooseLocation();
                            transform.position = teleportToLocation(currentLocation);
                            newProjectile = Instantiate(heatSeeking, transform.position, transform.rotation) as GameObject;
                            newProjectile.SetActive(true);
                            anim.SetTrigger("Shoot");
                            Debug.Log("Heat seeking");
                        }
                        if (lastAttack == 2)
                        {
                            currentLocation = 4;
                            transform.position = teleportToLocation(4);
                            Vector2 spawnPos = new Vector2();
                            if ((int)(Random.value * 2) == 1) spawnPos.x = -4;
                            else spawnPos.x = 20;
                            spawnPos.y = -1.4f;
                            newEnemy = Instantiate(minion, spawnPos, transform.rotation) as GameObject;
                            newEnemy.SetActive(true);
                            anim.SetTrigger("Summon");
                            Debug.Log("Summon guy");
                        }
                        if (lastAttack == 3)
                        {
                            currentLocation = 4;
                            transform.position = teleportToLocation(4);
                            newProjectile = Instantiate(fireWall1, transform.position, fireWall1.transform.rotation) as GameObject;
                            newProjectile.SetActive(true);
                            newProjectile = Instantiate(fireWall2, transform.position, fireWall2.transform.rotation) as GameObject;
                            newProjectile.SetActive(true);
                            newProjectile = Instantiate(fireWall3, transform.position, fireWall3.transform.rotation) as GameObject;
                            newProjectile.SetActive(true);
                            animTime = fireWallSummon;
                            anim.SetTrigger("Summon");
                            Debug.Log("Fire Cage");
                        }
                        numTeleports += (int)(Random.value * 3);
                        Debug.Log("TP Before next attack: " + (3 - numTeleports));
                        timeSinceLastAttack = timeToAttack;
                    }
                    timeSinceLastTeleport -= Time.deltaTime;
                    if (timeSinceLastTeleport < .4f)
                    {
                        anim.SetTrigger("TPOut");
                        animTime = .3f;
                    }
                }
            }
            else //Boss defeat behavior
            {
                //ReadyDialogue();
                GetComponent<BoxCollider2D>().isTrigger = true;
                DestroySpawns();
            }
            timeSinceLastAttack -= Time.deltaTime;
            animTime -= Time.deltaTime;
            anim.SetFloat("AnimTime", animTime);
            if (animTime < 0f)
            {
                anim.SetBool("Idle", true);
            }
            else
            {
                anim.SetBool("Idle", false);
            }
            //Want to have 3 attacks chosen using attack codes
            //1 = Heat-seeking shot
            //2 = Summon enemy
            //3 = Bullet rain


            //Want to base movement around teleporting between 4 different locations, chooses between them randomly while moving
            //Teleports 1 time before attacking, then teleports and attacks in phase 1
            //Teleports 1-3 times before attacking, then teleports and attacks in phase 2
            //Always teleports to location 4 for attacks 2 and 3
            //LOCATION KEY:
            //     [4]
            //  [2][1][3]
        }

    }

    public void ReadyDialogue()
    {
        npcDialogueToTurnOn.ready = true; 
    }
    
    private void DestroySpawns()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject en in enemiesInScene)
        {
            Destroy(en);
        }
    }

    public bool GetDead()
    {

        return dead; 
    }
}

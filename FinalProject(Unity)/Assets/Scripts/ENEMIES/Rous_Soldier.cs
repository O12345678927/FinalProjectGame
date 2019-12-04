﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Soldier : MonoBehaviour
{
    //public var
    [Header("Stats:")]
    public float speed;
    public float turningSpeed;
    public float health;  
    public int rousState;

    [Header("Controllers")]
    
    public Animator animator;    
    public GameObject[] blood;

    //private var    
    private Rigidbody2D rbody;
    private Vector3 targetDir; //target direction, weither it be the player that it is chasing, or a target position when wandering   
    private bool isAlive = true;
    private float detectionRange;
    private Transform playerPos;

    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        rbody = gameObject.GetComponent<Rigidbody2D>();        
        detectionRange = 7f;
    }
    private void FixedUpdate()
    {        
        RunCycle();
    }
    void RunCycle() // Decides what the rous is doing
    {
        if (isAlive)
        {
            // check if in range or was shot
            if (CheckForPlayer(detectionRange))
                rousState = 1;
            else if (rousState != 3) // inorder to idle the Rous must of not been eating 
            {
                detectionRange = 7f;
                rousState = 0;
            }
            // set the animation to the correct rousState
            animator.SetInteger("rousState", rousState);
            switch (rousState)
            {
                case 0: //Rous is Idle
                case 3: // Rous is Eating                         
                    break; // Do nothing
                case 1:     // Rous is Pursing                     
                    PointToTarget(FindTarget());
                    Move();
                    break;
                default:
                    break;
            }
        }
    }
    void PointToTarget(Vector3 currentTarget)
    {
        float angle;
        angle = Mathf.Atan2(currentTarget.y, currentTarget.x) * Mathf.Rad2Deg - 90f;
        //distance = Vector3.Distance(playerPos.position, transform.position); for future use to change turn speed based on how close the Rous is to the player
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turningSpeed);
    }
    void Move()
    {
        float angle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad; // get rous current angle        
        float sin = Mathf.Sin(angle); // get y trig ratio
        float cos = Mathf.Cos(angle); // get x trig ratio

        Vector3 forward = new Vector3(speed * cos, speed * sin, 0f); // turn direction into into a vector
        rbody.AddForce(forward * speed); // make the velocity to the direction Rous is facing
    }
    bool CheckForPlayer(float range)
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance < range)
            return true;
        else
        {
            detectionRange = 7;
            return false;
        }
    }
    Vector3 FindTarget()
    {
        return playerPos.position - transform.position;
    }
    public void HitByBullet(float damage)
    {
        
        health = health - damage;
        detectionRange = 12f;
        if (health < 0) // its dead now
        {
            isAlive = false;
            animator.SetBool("isDead", true);
            Destroy(rbody);
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
            
        }        
        else if (health < 40) // drop low damaged blood splatter
            Instantiate(blood[2], transform.position, transform.rotation);
        else if (health < 90) // drop medium damaged blood splatter
            Instantiate(blood[1], transform.position, transform.rotation);
        else // drop high damaged blood splatter
            Instantiate(blood[0], transform.position, transform.rotation);



    }    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Soldier : MonoBehaviour
{
    //public var
    [Header("Stats:")]
    public float speed;
    public float turningSpeed;
    public float health;
    public float damage;
    public int rousState;
    public float EATING_DETECTION_RANGE;
    public float NORMAL_DETECTION_RANGE;
    public float EXTENDED_DETECTION_RANGE;

    [Header("Controllers")]
    
    public Animator animator;    
    public GameObject[] blood;

    //private var    
    private Rigidbody2D rbody;
    private Vector3 targetDir; //target direction, weither it be the player that it is chasing, or a target position when wandering   
    private bool isAlive = true;
    private float detectionRange;
    private Transform playerPos;
    private const int KNOCKBACK_FORCE = 7500;    

    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        rbody = gameObject.GetComponent<Rigidbody2D>();
        detectionRange = NORMAL_DETECTION_RANGE;
    }
    private void FixedUpdate()
    {        
        RunCycle();
    }

    //-------------------------------------------------------Movement Functions-----------------------------------------
    void RunCycle() // Decides what the rous is doing
    {
        if (isAlive)
        {
            // check if in range or was shot
            if (CheckForPlayer(detectionRange)) //if player is in range
            {
                if (playerPos.gameObject.GetComponent<PlayerScript>().IsPlayerDead())// player is alive
                    rousState = 1;
                else if(!playerPos.gameObject.GetComponent<PlayerScript>().IsPlayerDead()) // player is dead
                {
                    Debug.Log("Player is dead");
                    if (CheckForPlayer(1.4f)) // if player is inrage of eating
                        rousState = 2;
                    else
                        rousState = 1;
                }
            }
            else if (rousState != 2) // inorder to idle the Rous must of not been eating 
            {
                detectionRange = NORMAL_DETECTION_RANGE;
                rousState = 0;
            }
            // set the animation to the correct rousState
            animator.SetInteger("rousState", rousState);
            switch (rousState)
            {
                case 0: //Rous is Idle
                    break;
                case 2: // Rous is Eating
                    detectionRange = EATING_DETECTION_RANGE;
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

    //-----------------------------------------------------Rous Behavouir functions------------------------------------------------
    bool CheckForPlayer(float range)
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance < range)
            return true;
        else
        {
            detectionRange = NORMAL_DETECTION_RANGE;
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
        detectionRange = EXTENDED_DETECTION_RANGE;
        if (health < 1) // its dead now
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
    private void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.gameObject.CompareTag("Player"))
        {
            if (playerPos.gameObject.GetComponent<PlayerScript>().IsPlayerDead())
            {
                rbody.AddForce(-gameObject.transform.up * KNOCKBACK_FORCE);                
            }
            else
                rousState = 2;
        }
    }
}

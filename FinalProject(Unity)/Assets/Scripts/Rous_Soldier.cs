using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Soldier : MonoBehaviour
{
    //public var
    public float speed, turningSpeed;
    public Transform playerPos;
    public Animator animator;
    public int rousState;

    //private var    
    private Rigidbody2D rbody;
    private Vector3 targetDir; //target direction, weither it be the player that it is chasing, or a target position when wandering   
    private float health;
    private float detectionRange;

    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        targetDir = new Vector3(playerPos.position.x - transform.position.x, playerPos.position.y - transform.position.y, 0f);
        health = 100f;
        detectionRange = 7f;
    }
    private void FixedUpdate()
    {
        Debug.Log("The Rous can see at a range of: " + detectionRange);
        RunCycle();
    }
    void RunCycle() // Decides what the rous is doing
    {

        // check if in range or was shot
        if (CheckForPlayer(detectionRange))
            rousState = 1;
        else
            rousState = 0;
        // set the animation to the correct rousState
        animator.SetInteger("rousState", rousState);
        switch (rousState)
        {
            case 0:    // Rous is Idle                         
                break;
            case 1:     // Rous is Pursing                     
                PointToTarget(FindTarget());
                Move();
                break;
            default:
                break;
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
    void Move()
    {
        float angle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad; // get rous current angle        
        float sin = Mathf.Sin(angle); // get y trig ratio
        float cos = Mathf.Cos(angle); // get x trig ratio

        Vector3 forward = new Vector3( speed * cos , speed * sin , 0f); // turn direction into into a vector
        rbody.velocity = forward; // make the velocity to the direction Rous is facing
    }
    Vector3 FindTarget()
    {
        return playerPos.position - transform.position;
    }
    public void HitByBullet(float damage)
    {
        Debug.Log("ROus has been hit");
        health -= damage;
        detectionRange = 11;
    }
}

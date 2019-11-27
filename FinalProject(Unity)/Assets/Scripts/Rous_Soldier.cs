using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Soldier : MonoBehaviour
{
    //public var
    public float speed, turningSpeed;
    public Transform playerPos;
    public Animator animator;
    public int rousState = 0;

    //private var    
    private Rigidbody2D rbody;
    private Vector3 targetDir; //target direction, weither it be the player that it is chasing, or a target position when wandering      
    

    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        targetDir = new Vector3(playerPos.position.x - transform.position.x, playerPos.position.y - transform.position.y, 0f);
    }
    private void FixedUpdate()
    {
        RunCycle();
    }
    void PointToTarget(Vector3 currentTarget)
    {
        float angle;
        angle = Mathf.Atan2(currentTarget.y, currentTarget.x) * Mathf.Rad2Deg - 90f;
        //distance = Vector3.Distance(playerPos.position, transform.position); for future use to change turn speed based on how close the Rous is to the player
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turningSpeed);
        
    }
    void RunCycle() // Decides what the rous is doing
    {
        animator.SetInteger("rousState", rousState);        
            switch (rousState)
            {
                case 0:     // Rous is Idle     
                    CheckForPlayer();
                    break;
                case 1:     // Rous is Pursing                
                    Vector3 targetDir = playerPos.position - transform.position;
                    PointToTarget(targetDir);
                    Move();
                    CheckForPlayer();
                    break;
                case 2:     // Rous is eating
                    break;
                default:
                    break;
            }
    }
    void CheckForPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance < 4)
            rousState = 1;
        else
        {
            rousState = 0;
            rbody.velocity = new Vector2(0f, 0f);
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
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Colliding");
        if (other.gameObject.CompareTag("Projectile"))
        {            
            Destroy(other.gameObject);
        }
    }
}

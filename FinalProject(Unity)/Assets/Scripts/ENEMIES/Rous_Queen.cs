using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Queen : MonoBehaviour
{
    //public var
    [Header("Stats:")]
    public float speed;
    public float turningSpeed;
    public float health;
    public float damage;   
    public int rousState;    
    public float NORMAL_DETECTION_RANGE;        

    [Header("Controllers")]

    public Animator animator;
    public GameObject[] blood;
    public float rotationValue;
    public bool dropBlood;
    public GameObject projectile;
    public GameObject[] launchPoints;
    public float fireRate;

    //private var    
    private Rigidbody2D rbody;
    private Vector3 targetDir; //target direction, weither it be the player that it is chasing, or a target position when wandering   
    private bool isAlive = true;
    private float detectionRange;
    private Transform playerPos;    
    private const int KNOCKBACK_FORCE = 7500;
    private const float FIRE_COOLDOWN = 1f;
    private const float WAVE_CYCLE_SCALE = 60f;
    private const float WAVE_SPREAD_SCALE = 25f;
    private const float WAVE_MINIMUM_ANGLE = 0.6666667f;
    private const float WAVE_ARM_SPREAD_SCALE = 7.5f;
    private float fireRateTimer = 0;
    private GameObject tempProjectile;


    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        rbody = gameObject.GetComponent<Rigidbody2D>();
        detectionRange = NORMAL_DETECTION_RANGE;


        Physics2D.IgnoreLayerCollision(0, 10); //For rous queen and the projectiles
        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(10, 10); 
    }
    void FixedUpdate()
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
                else if (!playerPos.gameObject.GetComponent<PlayerScript>().IsPlayerDead()) // player is dead
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
                    break; // Do nothing
                case 1:     // Rous is Pursing                     
                    PointToTarget(FindTarget());
                    Move();
                    if (fireRateTimer < Time.fixedDeltaTime)
                    {

                        fireRateTimer = FIRE_COOLDOWN;
                        ShootBalls();
                    }
                    else
                    {
                        fireRateTimer -= Time.fixedDeltaTime;
                    }
                    break;
                default:
                    break;
            }         
        }
    }
    void PointToTarget(Vector3 currentTarget)
    {
        float angle;
        angle = Mathf.Atan2(currentTarget.y, currentTarget.x) * Mathf.Rad2Deg + rotationValue;
        
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turningSpeed);
    }
    void Move()
    {
        
        rbody.AddForce(FindForwardVector() * speed); // make the velocity to the direction Rous is facing
    }
    public Vector3 FindForwardVector()
    {
        float angle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad; // get rous current angle        
        float sin = Mathf.Sin(angle); // get y trig ratio
        float cos = Mathf.Cos(angle); // get x trig ratio

        return new Vector3(speed * cos, speed * sin, 0f); // turn direction into into a vector}
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
        if (health < 1) // its dead now
        {
            isAlive = false;
            animator.SetBool("isDead", true);
            Destroy(rbody);
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());

        }
        else if (health < health/4 && dropBlood) // drop high damaged blood splatter
            Instantiate(blood[2], transform.position, transform.rotation);
        else if (health < health/2 && dropBlood) // drop medium damaged blood splatter
            Instantiate(blood[1], transform.position, transform.rotation);
        else if (dropBlood) // drop low damaged blood splatter
            Instantiate(blood[0], transform.position, transform.rotation);



    }
    void OnCollisionEnter2D(Collision2D collisionObj)
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

    //----------------------------------------------------Rous Shooting stuff-----------------------------------------------------
    void ShootBalls()
    {
        float spread;
        for (int i = 0; i < launchPoints.Length; i++)
        {
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    spread = -(Mathf.Sin(Time.time * Mathf.Deg2Rad * WAVE_CYCLE_SCALE) + WAVE_MINIMUM_ANGLE) * WAVE_SPREAD_SCALE - (3 - i) * WAVE_ARM_SPREAD_SCALE;
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    spread = (Mathf.Sin(Time.time * Mathf.Deg2Rad * WAVE_CYCLE_SCALE) + WAVE_MINIMUM_ANGLE) * WAVE_SPREAD_SCALE + (i - 4) * WAVE_ARM_SPREAD_SCALE;
                    break;
                default:
                    spread = 0;
                    break;
            }

            tempProjectile = Instantiate(projectile, launchPoints[i].transform.position, Quaternion.Euler(0f, 0f, 0f));
            tempProjectile.GetComponent<Poison_Ball>().Launch(spread);

        }      
    }
}

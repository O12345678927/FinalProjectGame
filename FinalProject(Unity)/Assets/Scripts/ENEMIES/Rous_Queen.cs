using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Queen : MonoBehaviour
{
    //public var
    public bool engagedWithPlayer = true;
    public float stateTime;
    public float fireRate;
    public GameObject poisonBall;
    public Sprite poisonBallImage;
    public Transform playerPos;
    public Transform[] launchPos;
    public Transform[] seekingLaunchPos;

    //private var
    private bool isAlive = true;
    private float currentTime = 0;

    private const float BULLET_SIZE = 0.333f;
    private const float SINE_FREQUENCY = 3.0f;
    private const float SINE_AMPLITUDE = 0.333f;
    readonly float[][] poisonBallDataArray = {  new float[] { 12.5f, 15, 1.0f}, // poison ball                                            
                                            new float[] { 7.5f, 20, 1.0f}};   // seeking posin ball
                                                                            //{velocity, damage, spread}

    void Start()
    {
        //poisonBall.GetComponent<SpriteRenderer>().sprite = poisonBallImage;
        //Destroy(poisonBall.transform.GetChild(0));
    }
    void FixedUpdate()
    {
        if (isAlive && engagedWithPlayer)
        {
            if (stateTime < Time.fixedDeltaTime)
            {
                stateTime = fireRate;
                LaunchBalls();
                Debug.Log("BALLS HAVE BEEN\nL A U N C H E D");
            }
        }

        if (stateTime > Time.fixedDeltaTime)
        {
            stateTime -= Time.fixedDeltaTime;
        }
        else
        {
            stateTime = 0;
        }
            
    }
    void LaunchBalls() // Launches the posinballs
    {        
        for (int i = 0; i < launchPos.Length; i++)
        {
            FireSlime(launchPos[i], Mathf.Sin(Time.time*SINE_FREQUENCY)*SINE_AMPLITUDE, poisonBallDataArray[0]);
        }
        //for (int i = 0; i < seekingLaunchPos.Length; i++)
        //{
            //Launch the Aimed poison balls
        //}
    }        
    Vector3 FindTarget()
    {
        return playerPos.position - transform.position;
    }
    void FireSlime(Transform transform, float spread, float[] weaponDataArray)
    {
        GameObject projectile;
        projectile = (GameObject)Instantiate(poisonBall, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
        Vector3 tempVec;
        tempVec = (-transform.up);
        float tempAngle = Mathf.Atan2(tempVec.y - transform.position.y, tempVec.x - transform.position.x) + spread;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(weaponDataArray[0] * Mathf.Cos(tempAngle), weaponDataArray[0] * Mathf.Sin(tempAngle));
        projectile.GetComponent<BulletBehaviour>().Initialize(weaponDataArray);
        projectile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());
    }
}

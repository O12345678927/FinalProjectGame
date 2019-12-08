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
    readonly float[][] poisonBallDataArray = {  new float[] { 20, 15, 1.0f}, // poison ball                                            
                                            new float[] { 15, 20, 1.0f}};   // seeking posin ball
                                                                                   //{velocity, damage, spread, coef, maxAmmo}

    void Start()
    {
        poisonBall.GetComponent<SpriteRenderer>().sprite = poisonBallImage;
    }
    // Update is called once per frame
    void Update()
    {
           
    }
    void FixedUpdate()
    {
        if (isAlive && engagedWithPlayer)
        {            
            if (currentTime >= fireRate)
            {                
                LaunchBalls();
                currentTime = fireRate;
            }
        }

        if (currentTime <= fireRate)
            currentTime++;
        else
            currentTime = 0f;
    }
    void LaunchBalls() // Launches the posinballs
    {        
        for (int i = 0; i < launchPos.Length; i++)
        {
            InitializeBullet(poisonBall, launchPos[i], 0f, poisonBallDataArray[0]);
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
    void InitializeBullet(GameObject bulletObject, Transform transform, float spread, float[] weaponDataArray)
    {
        Debug.Log("Initalizeing bullet");
        Vector3 tempVec = new Vector3(0f,-1f,0f);
        bulletObject.GetComponent<Rigidbody2D>().velocity = tempVec;
        bulletObject.GetComponent<BulletBehaviour>().Initialize(weaponDataArray);
    }
}

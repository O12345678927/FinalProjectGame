using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rous_Queen : MonoBehaviour
{
    //public var
    public bool engagedWithPlayer;   
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
            if (currentTime > fireRate)
            {
                // Launch poisonBalls
                currentTime = 0.0f;
            }
        }
    }
    void LaunchBalls() // Launches the posinballs
    {
        for (int i = 0; i < launchPos.Length; i++)
        {
            //Launch the Unaimed poison balls
        }
        for (int i = 0; i < seekingLaunchPos.Length; i++)
        {
            //Launch the Aimed poison balls
        }
    }        
    Vector3 FindTarget()
    {
        return playerPos.position - transform.position;
    }
}

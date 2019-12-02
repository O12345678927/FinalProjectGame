using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBehaviour : MonoBehaviour
{
    public int fadeInTime;
    public int fadeOutTime;
    public float spread;

    private uint lifeTime;
    private bool isMoving;
    private Vector2 velocity;

    void Start()
    {
        //velocity = new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread)).Normalize();
    }
    
    void Update()
    {
        if (isMoving)
        {
            //transform.position+=
        }
        if (lifetime < fadeInTime)
        {

        }
        else if (lifeTime > fadeOutTime)
        {

        }
    }
}

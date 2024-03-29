﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Ball : MonoBehaviour
{
    //public var
    public float poisonSpeed;
    public float damage;

    //Defining variables at start except calling them at start ends up in NullReferenceExceptions!

    public void Launch(float spread)
    {        
        Vector3 forward = GameObject.Find("RousQueen").GetComponent<Rous_Queen>().FindForwardVector().normalized;        
        forward = Quaternion.AngleAxis(spread, Vector3.forward) * forward;        
        gameObject.GetComponent<Rigidbody2D>().velocity = (forward * poisonSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}

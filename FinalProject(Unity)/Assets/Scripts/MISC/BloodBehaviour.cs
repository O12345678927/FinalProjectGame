using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BloodBehaviour : MonoBehaviour
{
    //public var
    public float speed;

    //private var
    private float time;
    private float scaleValue;
    private Vector2 direction; 
    private Vector3 scale;
    private Rigidbody2D rbody;

    void Start()
    {
        time = 5f;
        direction =  new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f,1.0f));        
        rbody = gameObject.GetComponent<Rigidbody2D>();
        speed = 200f;
        InitalizeRigidBody();
    }   
    
    private void FixedUpdate()
    {
        if (time > -6) // change size until the timer is over
        {
            scaleValue = -4f / 25f * Mathf.Pow(time, 2) + 8f;
            scale = new Vector3(scaleValue, scaleValue, 1f);
            transform.localScale = scale;
            time--;
            if (time == -5)
                BloodOnGround();
        }
    }
    void InitalizeRigidBody() //Im to lazy to edit all nine blood prefabs so Im doing it in a function
    {
        rbody.AddForce(direction * speed);
        rbody.drag = 10f;
        rbody.angularDrag = 10f;
        rbody.AddTorque(Random.Range(-1.0f, 1.0f) * 1000f); // add some spin
    }
    void BloodOnGround()
    {
         gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; // makes the blood render below items and entiys        
    }
}

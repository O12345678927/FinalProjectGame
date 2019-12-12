using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Ball : MonoBehaviour
{
    //public var
    public float speed;
    public float damage;

    //private var
    private Rigidbody2D rbody;
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        boss = GameObject.Find("RousQueen");
        Launch();
    }
    void Launch()
    {
        Vector3 forward = boss.GetComponent<Rous_Queen>().FindForwardVector();
        rbody.AddForce(forward * speed);
    }
    
}

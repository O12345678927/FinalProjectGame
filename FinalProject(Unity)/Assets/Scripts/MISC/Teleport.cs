using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 offset;
    public Transform destination;

    void OnTriggerEnter2D(Collider2D toPort)
    {
        toPort.transform.position = destination.position + offset;
        Debug.Log(toPort.transform.position);
    }
}

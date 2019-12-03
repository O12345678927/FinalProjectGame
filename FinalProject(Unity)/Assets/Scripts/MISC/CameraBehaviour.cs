using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //public var
    public Transform cameraTarget;
    
    //private var
    private Vector3 cameraPosition;
   
    void Update()
    {
        cameraPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, -10f);
        gameObject.transform.position = cameraPosition;
        
    }
}

   

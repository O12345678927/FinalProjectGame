using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    // Start is called before the first frame update
    //public var
    public Camera mainCamera;
    public float changeRate;    
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {           
            mainCamera.GetComponent<CameraBehaviour>().SetChangeRate(changeRate);
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {           
            mainCamera.GetComponent<CameraBehaviour>().SetChangeRate(-changeRate);
        }
    }
}

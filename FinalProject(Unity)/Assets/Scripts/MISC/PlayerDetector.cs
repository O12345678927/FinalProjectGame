using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    // Start is called before the first frame update
    //public var
    public Camera mainCamera;
    public float changeRate;
    public GameObject boss;
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("EnterBossArea");
            mainCamera.GetComponent<CameraBehaviour>().SetChangeRate(changeRate);
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ExitBossArea");
            mainCamera.GetComponent<CameraBehaviour>().SetChangeRate(-changeRate);
        }
    }
}

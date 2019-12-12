using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //public var
    public Transform cameraTarget;
    
    //private var
    private Vector3 cameraPosition;
    private float changeRate = 0;


    void Update()
    {
        cameraPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, -10f);
        gameObject.transform.position = cameraPosition;
        AdjustCameraSize();        
    }
    void AdjustCameraSize()
    {
        if (changeRate > 0) // camera is increasing in size
        {
            Debug.Log("Increase size of camera");
            if (gameObject.GetComponent<Camera>().orthographicSize < 8)
                gameObject.GetComponent<Camera>().orthographicSize += changeRate;
            else
                SetChangeRate(0.0f);
        }
        else if (changeRate < 0) // camera is decreasing in size
        {
            Debug.Log("Decrease size of Camera");
            if(gameObject.GetComponent<Camera>().orthographicSize > 5)// check if camera is the correct size
                gameObject.GetComponent<Camera>().orthographicSize += changeRate;
            else
                SetChangeRate(0.0f);
        }
    }
    public void SetChangeRate(float newChangeRate)
    {
        //Debug.Log("Changing the change rate of the camera to: " + newChangeRate);
        changeRate = newChangeRate;
    }

}

   

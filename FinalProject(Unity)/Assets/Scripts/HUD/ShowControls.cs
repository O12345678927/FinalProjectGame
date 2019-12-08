using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControls : MonoBehaviour
{
    public Image controlsBackground;
    public Image controls;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisableText", 10f); // Close the controls window after 10 seconds
    }

    void DisableText()
    {
        controls.enabled = false;
        controlsBackground.enabled = false;
    }
}

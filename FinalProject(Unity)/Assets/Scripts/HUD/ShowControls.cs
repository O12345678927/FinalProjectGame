using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControls : MonoBehaviour
{
    public Image controlsBackground;
    public Image controls;

    private Image startScreen;
    private Image start;

    // Start is called before the first frame update
    void Start()
    {
        startScreen = GameObject.Find("StartScreen").GetComponentInChildren<Image>();
        start = GameObject.Find("Start").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CloseStart();
            Invoke("DisableText", 10f); // Close the controls window after 10 seconds
        }
    }

    void DisableText()
    {
        controls.enabled = false;
        controlsBackground.enabled = false;
    }

    void CloseStart()
    {
        startScreen.enabled = false;
        start.enabled = false;
    }
}

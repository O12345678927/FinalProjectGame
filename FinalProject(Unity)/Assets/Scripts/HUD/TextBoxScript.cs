using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxScript : MonoBehaviour
{
    [Header("Message Properties")]
    public bool isMessage;
    public bool wasTriggered;
    public string message;
    public float MessageTime;

    [Header("TeleporterProperties")]
    public bool isTeleporter;
    public bool isLocked; //default true
    public Vector3 offset;
    public Transform destination;

    //private vars
    private GameObject player;
    private Image textBox;
    private Image border; 
    private Text text;

    // Start is called before the first frame update
    void Start()
    {        
        player = GameObject.Find("Player");
        textBox = GameObject.Find("TextBox").GetComponentInChildren<Image>();
        border = GameObject.Find("Border").GetComponent<Image>();
        text = GameObject.Find("Text").GetComponent<Text>();
        HideTextBox(); // Hidden on game start
    }

    void HideTextBox()
    {
        textBox.enabled = false;
        border.enabled = false;
        text.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isMessage)
        {
            if (other.CompareTag("Player") && !wasTriggered)
            {
                ShowTextBox(message, MessageTime);
                wasTriggered = true;
            }
        }
        if (isTeleporter && !isLocked)
        {
            other.transform.position = destination.position + offset;
            Debug.Log(other.transform.position);
        }
    }

    // Call this to show text: (string [text you want to output], float [how many seconds it will stay on screen])
    public void ShowTextBox(string newText, float seconds)
    {
        textBox.enabled = true;
        border.enabled = true;
        text.enabled = true;
        text.text = newText;

        // Closes the textbox after {seconds} seconds
        Invoke("HideTextBox", seconds);
    }       
}

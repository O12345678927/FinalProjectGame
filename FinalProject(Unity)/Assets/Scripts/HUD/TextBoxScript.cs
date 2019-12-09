using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxScript : MonoBehaviour
{
    public bool wasTriggered;
    public string message;
    public float MessageTime;

    public GameObject player;
    public Image textBox; // Drag TextBox here
    public Image border; // Drag Border here
    public Text text; // Drag Text here

    // Start is called before the first frame update
    void Start()
    {
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
        if (other.CompareTag("Player") && !wasTriggered)
        {
            ShowTextBox(message, MessageTime);
            wasTriggered = true;
        }    
    }

    // Call this to show text: (string [text you want to output], float [how many seconds it will stay on screen])
    void ShowTextBox(string newText, float seconds)
    {
        textBox.enabled = true;
        border.enabled = true;
        text.enabled = true;
        text.text = newText;

        // Closes the textbox after {seconds} seconds
        Invoke("HideTextBox", seconds);
    }    
}

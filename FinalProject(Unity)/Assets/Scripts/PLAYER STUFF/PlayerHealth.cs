using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider HealthBar; // Drag Health Bar game object here
    public float maxHealth = 100;

    private float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        // At the start of the game the player has full health
        currentHealth = maxHealth;
    }

    private void Update()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
        currentHealth = playerScript.health;
        //HealthBar.value = currentHealth;
    }
}

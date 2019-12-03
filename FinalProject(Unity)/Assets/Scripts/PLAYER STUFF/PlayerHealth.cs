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

    // Can call this in another script, intakes a damage number and updates the Health Bar accordingly
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        HealthBar.value = currentHealth;
    }
}

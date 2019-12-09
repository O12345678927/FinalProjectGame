using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    [Header("Ammo")]
    public Text rifleAmmo; // Drag RifleAmmo here
    public Text shotgunAmmo; // Drag ShotgunAmmo here

    [Header("Equip")]
    public Image pistolEquipped; // Drag Pistol image here
    public Image rifleEquipped; // Drag Rifle image here
    public Image shotgunEquipped; // Drag Shotgun image here

    // Start is called before the first frame update
    void Start()
    {
        // At the start all weapons are unequipped (color is black)
        pistolEquipped.color = new Color32(0, 0, 0, 255);
        rifleEquipped.color = new Color32(0, 0, 0, 255);
        shotgunEquipped.color = new Color32(0, 0, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();

        // Updates rifle and shotgun ammo amounts
        rifleAmmo.text = playerScript.inventory[2, 1].ToString();
        shotgunAmmo.text = playerScript.inventory[3, 1].ToString();

        // Shows the currently equipped weapon in the HUD (color to white)
        if (Input.GetKey(KeyCode.Alpha1)) // Unarmed
        {
            if (playerScript.inventory[1, 0] == 1) // Player has pistol
                pistolEquipped.color = new Color32(70, 120, 130, 255); // (colour to grey-blue)
            else
                pistolEquipped.color = new Color32(0, 0, 0, 255); // (colour to black)
            if (playerScript.inventory[2, 0] == 1)
                rifleEquipped.color = new Color32(70, 120, 130, 255);
            else
                rifleEquipped.color = new Color32(0, 0, 0, 255);
            if (playerScript.inventory[3, 0] == 1)
                shotgunEquipped.color = new Color32(70, 120, 130, 255);
            else
                shotgunEquipped.color = new Color32(0, 0, 0, 255);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && playerScript.inventory[1, 0] == 1) // Pistol
        {
            pistolEquipped.color = new Color32(255, 255, 255, 255);

            if (playerScript.inventory[2, 0] == 1)
                rifleEquipped.color = new Color32(70, 120, 130, 255);
            else
                rifleEquipped.color = new Color32(0, 0, 0, 255);
            if (playerScript.inventory[3, 0] == 1)
                shotgunEquipped.color = new Color32(70, 120, 130, 255);
            else
                shotgunEquipped.color = new Color32(0, 0, 0, 255);
        }
        else if (Input.GetKey(KeyCode.Alpha3) && playerScript.inventory[2, 0] == 1) // Rifle
        {
            rifleEquipped.color = new Color32(255, 255, 255, 255);

            if (playerScript.inventory[1, 0] == 1)
                pistolEquipped.color = new Color32(70, 120, 130, 255);
            else
                pistolEquipped.color = new Color32(0, 0, 0, 255);
            if (playerScript.inventory[3, 0] == 1)
                shotgunEquipped.color = new Color32(70, 120, 130, 255);
            else
                shotgunEquipped.color = new Color32(0, 0, 0, 255);
        }
        else if (Input.GetKey(KeyCode.Alpha4) && playerScript.inventory[3, 0] == 1) // Shotgun
        {
            shotgunEquipped.color = new Color32(255, 255, 255, 255);

            if (playerScript.inventory[1, 0] == 1)
                pistolEquipped.color = new Color32(70, 120, 130, 255);
            else
                pistolEquipped.color = new Color32(0, 0, 0, 255);
            if (playerScript.inventory[2, 0] == 1)
                rifleEquipped.color = new Color32(70, 120, 130, 255);
            else
                rifleEquipped.color = new Color32(0, 0, 0, 255);
        }
    }
}

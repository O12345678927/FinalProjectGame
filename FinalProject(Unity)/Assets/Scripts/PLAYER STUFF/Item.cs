using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //public var
    public int itemType; // 0 = Health, 1 = pistol, 2 rifle, 3 = shotgun
    public int quantity; // value that the item has in it
    public bool isWeapon;

    //private var
    private GameObject player;
    
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            player.GetComponent<PlayerScript>().PickupItem(itemType, isWeapon, quantity);
            Destroy(gameObject);
        }
    }
}

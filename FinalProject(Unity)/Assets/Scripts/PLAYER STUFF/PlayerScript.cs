using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public var      
    [Header("Playerstats")]
    public float speed;
    public float health;
    public float invulnerabilityTime;

    [Header("Controllers")]
    public Object devBullet;
    public GameObject bloodEffects;
    public Animator animator;
    public GameObject[] blood;
    public Camera mainCamera;

    [Header("Bullet Sprites")]
    public Sprite projectileSpritePisol;
    public Sprite projectileSpriteRifle;
    public Sprite projectileSpriteShotgun;

    [Header("Inventory")]
    public int[,] inventory; // [integer = if player has weapon 1 = yes, 0 = no][integer = amount of ammo weapon has]

    //private
    private Rigidbody2D rbody;
    private float horiz, vert;
    private ushort weaponIndex = 0;
    private float damageTimer = 0;
    private bool playerIsAlive = true;
    private bool playerIsFrozen = false; // use for cutscenes or scripted scences

    private GameObject bulletObject;
    private float chamberTime = 0;

    readonly float[][] weaponDataArray = {  new float[] { 0, 0, 0, 0 },         //Unarmed
                                            new float[] { 50, 28f, 0.95f, -1},     //pistol
                                            new float[] { 66, 52f, 0.97f, 256},     //rifle
                                            new float[] { 100, 30f, 0.75f, 64}};   //shotgun
                                                                                   //{velocity, damage, coef, maxAmmo}
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        inventory = new int[4, 2];
        for (int xx = 0; xx < 4; xx++) //loops thourgh inventory and disables all weapons and sets ammo to zero. 
        {            
                inventory[xx, 0] = 0;
                inventory[xx, 1] = 0;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Player"));
    }
    void FixedUpdate()
    {
        if (playerIsAlive)
            MovePlayer();
        if (damageTimer > Time.fixedDeltaTime)        
            damageTimer -= Time.fixedDeltaTime;       
        else
            damageTimer = 0f;        
    }
    void Update()
    {
        if (playerIsAlive)
        {
            ChangeDirection();
            WeaponListener();
        }
    }

    //--------------------------------- Movement functions ----------------------------------------
    void ChangeDirection() // changes the angle of the player to face the mouse
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
    void IsMoving() // Checks if player is moving, if so change the bool condition in the animator
    {
        if (horiz > 0 || horiz < 0 || vert > 0 || vert < 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }
    void MovePlayer()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        Vector2 playerDir = new Vector2(horiz, vert);
        playerDir.Normalize();
        rbody.AddForce(playerDir * speed);
        IsMoving();
    }

    //--------------------------------- Weapon functions ----------------------------------------
    void CheckInput(ref ushort weaponIndex)
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetInteger("selectedWeapon", 0); //Unarmed
            weaponIndex = 0; //returns Unarmed (0)
        }
        else if (Input.GetKey(KeyCode.Alpha2) && inventory[1, 0] == 1)
        {
            animator.SetInteger("selectedWeapon", 1); //pistol
            weaponIndex = 1; //returns Pistol (1)
        }
        else if (Input.GetKey(KeyCode.Alpha3) && inventory[2, 0] == 1)
        {
            animator.SetInteger("selectedWeapon", 2); //rifle
            weaponIndex = 2; //returns Rifle (2)
        }
        else if (Input.GetKey(KeyCode.Alpha4) && inventory[3, 0] == 1)
        {
            animator.SetInteger("selectedWeapon", 3); //Shotgun
            weaponIndex = 3; //returns Shotgun (3)
        }
    }
    void InitializeBullet(GameObject bulletObject, Transform transform, float spread, float[] weaponDataArray)
    {
        Vector3 tempVec;
        tempVec = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float tempAngle = Mathf.Atan2(tempVec.y - transform.position.y, tempVec.x - transform.position.x) + spread;
        bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(weaponDataArray[0] * Mathf.Cos(tempAngle), weaponDataArray[0] * Mathf.Sin(tempAngle));
        bulletObject.GetComponent<BulletBehaviour>().Initialize(weaponDataArray);
    }
    void FireWeapon(ushort inputType, Transform transform, float spread, float[] weaponDataArray)
    {
        /*
         * Types:
         *      0 - Debug projectile
         *      1 - Bullet (Pistol)
         *      2 - Bullet (Rifle)
         *      3 - Pellet (Shotgun)
         */
        bulletObject = (GameObject)Instantiate(devBullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
        InitializeBullet(bulletObject, transform, spread, weaponDataArray);
        switch (inputType)
        {
            case 0:
                //Case 0 is unused, can be cleaned up when it seems fit
                bulletObject = (GameObject)Instantiate(devBullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
                InitializeBullet(bulletObject, transform, spread, weaponDataArray);
                break;
            case 1:
                bulletObject.GetComponent<SpriteRenderer>().sprite = projectileSpritePisol;
                break;
            case 2:
                bulletObject.GetComponent<SpriteRenderer>().sprite = projectileSpriteRifle;
                break;
            case 3:
                bulletObject.GetComponent<BulletBehaviour>().UseTurbulence(true, spread);
                bulletObject.GetComponent<SpriteRenderer>().sprite = projectileSpriteShotgun;
                break;
            default:
                Debug.Log("Error, invalid bullet type called!");
                break;
        }
    }
    void WeaponListener()
    {
        CheckInput(ref weaponIndex);
        switch (weaponIndex)
        {
            case 1:
                if (Input.GetButtonDown("Fire1") && chamberTime < Time.deltaTime)
                {
                    FireWeapon(1, transform, Random.Range(-0.05f, 0.05f), weaponDataArray[1]);
                    chamberTime = 0.15f;
                    
                }
                else
                {
                    if (chamberTime > Time.deltaTime)
                        chamberTime -= Time.deltaTime;
                    else
                        chamberTime = 0;
                }
                break;
            case 2:
                if (Input.GetButton("Fire1") && chamberTime < Time.deltaTime && inventory[2, 1] > 0)
                {
                    FireWeapon(2, transform, Random.Range(-0.1f, 0.1f), weaponDataArray[2]);
                    chamberTime = 0.18f;
                    inventory[2, 1]--;
                }
                else
                {
                    if (chamberTime > Time.deltaTime)
                        chamberTime -= Time.deltaTime;
                    else
                        chamberTime = 0;
                }
                break;
            case 3:
                if (Input.GetButton("Fire1") && chamberTime < Time.deltaTime && inventory[3, 1] > 0)
                {
                    for (int x = 0; x < 8; x++)
                        FireWeapon(3, transform, Random.Range(-0.05f, 0.05f), weaponDataArray[3]);
                    chamberTime = 0.9f;
                    inventory[3, 1]--;
                }
                else
                {
                    if (chamberTime > Time.deltaTime)
                        chamberTime -= Time.deltaTime;
                    else
                        chamberTime = 0;
                }
                break;
            default:
                break;

        }
    }

    //--------------------------------- Damage functions ----------------------------------------
    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Enemy")|| other.gameObject.CompareTag("Projectile")) && damageTimer < Time.fixedDeltaTime)
        {
            damageTimer = invulnerabilityTime;            
            Vector3 recoil;
            GameObject hitBy = other.gameObject;

            recoil = transform.position - hitBy.transform.position;
            recoil.Normalize();

            if (!(hitBy.GetComponent(typeof(Rous_Soldier)) == null))
            {
                ApplyDamage(hitBy.GetComponent<Rous_Soldier>().damage, recoil * 10000f);
            }
            else if (!(hitBy.GetComponent(typeof(Fauna)) == null))
            {
                ApplyDamage(1f, recoil * 2500f);
            }
            else if (!(hitBy.GetComponent(typeof(Rous_Queen)) == null))
            {
                ApplyDamage(hitBy.GetComponent<Rous_Queen>().damage, recoil * 5500f);
            }else if (!(hitBy.GetComponent(typeof(Poison_Ball)) == null))
            {
                Debug.Log("Hit by poison ball");
                ApplyDamage(hitBy.GetComponent<Poison_Ball>().damage, recoil * 1000f);
                Destroy(hitBy);
            }
        }
    }     
    void ApplyDamage(float amount, Vector3 recoil)
    {
        health -= amount;
        
        if (health < 1) // its dead now
        {
            playerIsAlive = false;
            animator.SetBool("isDead", true);
            Destroy(rbody);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if (health < 25) // drop low damaged blood splatter
            Instantiate(blood[2], transform.position, transform.rotation);
        else if (health < 75) // drop medium damaged blood splatter
            Instantiate(blood[1], transform.position, transform.rotation);
        else // drop high damaged blood splatter
            Instantiate(blood[0], transform.position, transform.rotation);
    }
    public void PickupItem(int weaponIndex, bool isWeapon, int ammoQuantity)
    {
        // [integer = if player has weapon 1 = yes, 0 = no][integer = amount of ammo weapon has]
        if(weaponIndex == 0) // first check if the item being picked up is a healthpack
        {
            health = Mathf.Min(ammoQuantity + health, 100f);
        }
        else if (isWeapon) // The item being picked up is a weapon, unlock it and add ammo
        {
            inventory[weaponIndex, 0] = 1; // The weapon is selected
            inventory[weaponIndex, 1] += ammoQuantity;
            Debug.Log("Ammo of Ammo type" + weaponIndex + " = " + inventory[weaponIndex, 1]);
        }
        else // if the item is not a weapon or a health pack then it is ammo
        {           
            inventory[weaponIndex, 1] = (int)Mathf.Min(inventory[weaponIndex, 1] + ammoQuantity, weaponDataArray[weaponIndex][3]);
        }
    }
    public bool IsPlayerDead()
    {       
        return playerIsAlive;
    }
    
   

}

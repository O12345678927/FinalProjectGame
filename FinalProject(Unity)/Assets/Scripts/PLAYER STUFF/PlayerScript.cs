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

    [Header("Bullet Sprites")]
    public Sprite projectileSpritePisol;
    public Sprite projectileSpriteRifle;
    public Sprite projectileSpriteShotgun;

    //private
    private Rigidbody2D rbody;
    private float horiz, vert;
    private ushort weaponIndex = 0;
    private float damageTimer = 0;
    private object[,] inventory; // (x,y) x is for each weapons ammo, y is a boolean if the weapon has been picked up
    private bool playerIsAlive = true;
    private bool playerIsFrozen = false; // use for cutscenes or scripted scences

    private GameObject bulletObject;
    private float chamberTime = 0;

    readonly float[][] weaponDataArray = {  new float[] { 0, 0, 0, 0, 0 },         //Unarmed
                                            new float[] { 50, 25f, 0.95f, -1 },     //pistol
                                            new float[] { 66, 44f, 0.97f, 120},     //rifle
                                            new float[] { 100, 30f, 0.75f, 36}};   //shotgun
                                                                                //{velocity, damage, spread, coef, maxAmmo}

    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        inventory = new object[4, 2];
        for (int xx = 0; xx < 4; xx++) //loops thourgh inventory and {CURRENTLY ENABLES!!!!!!!}disables all weapons and sets ammo to zero. 
        {
            if (xx == 0) //You've got fists I guess
            {
                inventory[xx, 0] = true;
                inventory[xx, 1] = 0;
            }
            else //Set everything else to 0
            {
                inventory[xx, 0] = false;
                inventory[xx, 1] = 0;
            }

        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Player"));


    }
    void FixedUpdate()
    {
        if (playerIsAlive && !playerIsFrozen)
        {
            movePlayer();
        }

        if (damageTimer > Time.fixedDeltaTime)
        {
            damageTimer -= Time.fixedDeltaTime;
            //Debug.Log($"{damageTimer} - {Time.fixedDeltaTime}");
        }
        else
        {
            damageTimer = 0f;
        }
    }
    void Update()
    {
        if (playerIsAlive && !playerIsFrozen)
        {
            ChangeDirection();
            WeaponListener();
        }
        //Debug.Log("Weapon number is" + weaponIndex);
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
    void movePlayer()
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
        else if (Input.GetKey(KeyCode.Alpha2) && (bool)inventory[1, 0])
        {
            animator.SetInteger("selectedWeapon", 1); //pistol
            weaponIndex = 1; //returns Pistol (1)
        }
        else if (Input.GetKey(KeyCode.Alpha3) && (bool)inventory[2, 0])
        {
            animator.SetInteger("selectedWeapon", 2); //rifle
            weaponIndex = 2; //returns Rifle (2)
        }
        else if (Input.GetKey(KeyCode.Alpha4) && (bool)inventory[3, 0])
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
                if (Input.GetButton("Fire1") && chamberTime < Time.deltaTime)
                {
                    FireWeapon(2, transform, Random.Range(-0.1f, 0.1f), weaponDataArray[2]);
                    chamberTime = 0.12f;
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
                if (Input.GetButton("Fire1") && chamberTime < Time.deltaTime)
                {
                    for (int x = 0; x < 8; x++)
                        FireWeapon(3, transform, Random.Range(-0.05f, 0.05f), weaponDataArray[3]);
                    chamberTime = 0.9f;
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
        if (other.gameObject.CompareTag("Enemy") && damageTimer < Time.fixedDeltaTime)
        {
            damageTimer = invulnerabilityTime;
            Debug.Log($"Invulnerability time!\n{invulnerabilityTime} seconds");
            Vector3 recoil;
            GameObject hitBy = other.gameObject;
            if (!(hitBy.GetComponent(typeof(Rous_Soldier)) == null))
            {
                recoil = transform.position - hitBy.transform.position;
                recoil.Normalize();
                ApplyDamage(10f, recoil * 10000f);
            }
            else if (!(hitBy.GetComponent(typeof(Fauna)) == null))
            {
                recoil = transform.position - hitBy.transform.position;
                recoil.Normalize();
                ApplyDamage(1f, recoil * 2500f);
            }
            //Add more for poin balls
        }
    }
    void ApplyDamage(float amount, Vector3 recoil)
    {
        if (health > 0)
        {
            health -= amount;
            rbody.AddForce(recoil);
        }
        if (health < 1) // player is killed
        {
            playerIsAlive = false;
            Destroy(rbody);
            animator.SetBool("isDead", true);
        }
    }
    public void PickupResource(uint weaponIndex, bool isWeapon, uint ammoQuantity)
    {
        if ((bool)inventory[weaponIndex, 0])
        {
            inventory[weaponIndex, 1] = Mathf.Min((short)inventory[weaponIndex, 1] + ammoQuantity,weaponDataArray[weaponIndex][3]);
        }
        else
        {
            inventory[weaponIndex, 0] = weaponIndex;
            inventory[weaponIndex, 1] = Mathf.Min((short)inventory[weaponIndex, 1] + ammoQuantity, weaponDataArray[weaponIndex][3]);
        }
    }
}

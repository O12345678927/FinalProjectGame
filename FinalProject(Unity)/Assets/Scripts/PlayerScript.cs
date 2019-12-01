using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public var
    public float speed;
    public Animator animator;
    public Object devBullet;
    public GameObject bloodEffects;

    public Sprite projectileSpritePisol;
    public Sprite projectileSpriteRifle;
    public Sprite projectileSpriteShotgun;

    //private
    private ushort weaponIndex = 0;
    private Rigidbody2D rbody;
    private float horiz, vert;
    private int[,] inventory; // (x,y) x is for each weapons ammo, y is a boolean if the weapon has been picked up
    private GameObject bulletObject;
    private float chamberTime = 0;

    readonly float[][] weaponDataArray = {    new float[] { 0, 0, 0, 0 },        //fists
                                                new float[] { 40, 15, 0.97f },      //pistol
                                                new float[] { 45, 30, 0.99f },      //rifle
                                                new float[] { 100, 7.5f, 0.85f }};    //shotgun
                                                //{velocity, damage, spread, coef}

    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        inventory = new int[4, 2];
        for (int xx = 0; xx < 4; xx++) //loops thourgh inventory and disables all weapons and sets ammo to zero. 
        {
            for (int yy = 0; yy < 2; yy++) // row 0 is redundent, as it is unarmed, no ammo needed
                inventory[xx, yy] = 1;
        }
        
    }
    private void Update()
    {
        IsMoving();
        CheckInput(ref weaponIndex);    
        switch (weaponIndex)
        {
            case 1:
                if (Input.GetButtonDown("Fire1") && chamberTime<Time.deltaTime)
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
                    for (int x = 0; x < 6; x++)
                        FireWeapon(3, transform, Random.Range(-0.03f, 0.03f), weaponDataArray[3]);                    
                    chamberTime = 1.2f;
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
                //I dunno'      Punch maybe lol
                break;

        }
    }
    void FixedUpdate()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        Vector2 playerDir = new Vector2(horiz, vert);
        playerDir.Normalize();
        ChangeDirection();
        rbody.AddForce(playerDir * speed);
    }
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
    void CheckInput(ref ushort weaponIndex)
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetInteger("selectedWeapon", 0); //Unarmed
            weaponIndex = 0; //returns Unarmed (0)
        }
        else if (Input.GetKey(KeyCode.Alpha2) && inventory[1,1] == 1)
        {
            animator.SetInteger("selectedWeapon", 1); //pistol
            weaponIndex = 1; //returns Pistol (1)
        }
        else if (Input.GetKey(KeyCode.Alpha3) && inventory[2, 1] == 1)
        {
            animator.SetInteger("selectedWeapon", 2); //rifle
            weaponIndex = 2; //returns Rifle (2)
        }
        else if (Input.GetKey(KeyCode.Alpha4) && inventory[3, 1] == 1)
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
                //A bunch of temporary bullshit, this will be removed
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
}

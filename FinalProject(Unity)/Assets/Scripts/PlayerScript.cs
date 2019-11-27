using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public var
    public float speed;
    public float temporaryBulletSpeed;
    public Animator animator;
    public Object devBullet;

    //private
    private ushort weaponIndex = 0;
    private Rigidbody2D rbody;
    private float horiz, vert;
    private int[,] inventory; // (x,y) x is for each weapons ammo, y is a boolean if the weapon has been picked up
    private GameObject bulletObject;
    private float chamberTime = 0;


    // Start is called before the first frame update
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
        
        //This is temporary       
        switch (weaponIndex)
        {
            case 1:
                if (Input.GetButtonDown("Fire1") && chamberTime<Time.deltaTime)
                {
                    FireWeapon(0, temporaryBulletSpeed, transform, Random.Range(-1.0f, 1.0f));
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
                    FireWeapon(0, temporaryBulletSpeed, transform, Random.Range(-1.0f, 1.0f));
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
                    {
                        FireWeapon(0, temporaryBulletSpeed, transform, Random.Range(-0.2f, 0.2f));
                    }
                    chamberTime = 1.2f;
                }
                else
                {
                    if (chamberTime > Time.deltaTime)
                    {
                        chamberTime -= Time.deltaTime;
                    }
                    else
                    {
                        chamberTime = 0;
                    }
                }
                break;
            default:
                //I dunno'      Punch maybe lol
                break;

        }
            
        ////A bunch of temporary bullshit, this will be removed
        //bulletObject = (GameObject)Instantiate(Resources.Load("Prefabs/devbullet"), transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
        //Vector3 tempVec = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        //tempVec.z = 0;
        //tempVec = Vector3.Normalize(tempVec);
        //bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(temporaryBulletSpeed * tempVec.x, temporaryBulletSpeed * tempVec.y);

        //Debug.Log($"Rotation: {transform.rotation.x},{transform.rotation.y},{transform.rotation.z},{transform.rotation.w}");
        ////bulletObject.GetComponent<Rigidbody2D>().rotation =


        

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        ChangeDirection();
        rbody.velocity = new Vector2(horiz * speed, vert * speed);
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
    void FireWeapon(ushort inputType, float velocity, Transform transform, float spread)
    {
        /*
         * Types:
         *      0 - Debug projectile
         *      1 - Bullet nospread
         *      2 - Bullet w/spread
         *      3 - Pellet nospread
         *      4 - Pellet w/spread
         */
        switch (inputType)        
        {        
            case 0:
                //A bunch of temporary bullshit, this will be removed
                bulletObject = (GameObject)Instantiate(devBullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
                Vector3 tempVec = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float tempAngle = Mathf.Atan2(tempVec.y - transform.position.y, tempVec.x - transform.position.x) + spread;
                bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(temporaryBulletSpeed * Mathf.Cos(tempAngle), temporaryBulletSpeed * Mathf.Sin(tempAngle));
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                Debug.Log("Error, invalid bullet type called!");
                break;
        }
        
    }
}

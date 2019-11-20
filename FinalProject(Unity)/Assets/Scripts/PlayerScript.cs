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
    private Rigidbody2D rbody;
    private float horiz, vert;
    private int[,] inventory; // (x,y) x is for each weapons ammo, y is a boolean if the weapon has been picked up
    private GameObject bulletObject;
    
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
        CheckInput();

        //This is temporary
        if (Input.GetButtonDown("Fire1"))
        {
            //A bunch of temporary bullshit, this will be removed
            bulletObject = (GameObject)Instantiate(Resources.Load("Prefabs/devbullet"),transform.position, transform.rotation);
            Vector3 tempVec = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            tempVec.z = 0;
            tempVec = Vector3.Normalize(tempVec);
            bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(temporaryBulletSpeed * tempVec.x, temporaryBulletSpeed * tempVec.y); 
            
        }

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

    void CheckInput()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetInteger("selectedWeapon", 0); //Unarmed
        }
        else if (Input.GetKey(KeyCode.Alpha2) && inventory[1,1] == 1)
        {
            animator.SetInteger("selectedWeapon", 1); //pistol
        }
        else if (Input.GetKey(KeyCode.Alpha3) && inventory[2, 1] == 1)
        {
            animator.SetInteger("selectedWeapon", 2); //rifle
        }
        else if (Input.GetKey(KeyCode.Alpha4) && inventory[3, 1] == 1)
        {
            animator.SetInteger("selectedWeapon", 3); //Shotgun
        }
    }
}

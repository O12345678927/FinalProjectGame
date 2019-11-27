using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float DRAG_COEF = 0.97f;
    public float tracerSize = 0.125f;

    Rigidbody2D selfRigidBody;
    SpriteRenderer selfSpriteRenderer;

    private float physAbsVelocity;
    private float physDirection;
    private ushort lifeTime;
    
    // Start is called before the first frame update
    // use ignorecollision and tags/labels!!!!!!
    void Start()
    {
        if ((GetComponent(typeof(BoxCollider2D)) == null) || (GetComponent(typeof(Rigidbody2D)) == null)) {
            Debug.Log($"Bullet without box collider or rigid body...\nRemoving object {gameObject.name.ToString()}");
            Destroy(gameObject);
        }
        else
        {
            selfRigidBody = GetComponent<Rigidbody2D>();

            //If the sprite renderer doesn't exist then I don't even care anymore, I'm gonna assign this without checking anyways
            selfSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        foreach (GameObject curObject in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            //Ignore physics for each
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), curObject.GetComponent<BoxCollider2D>());
            Debug.Log(curObject.gameObject.name);
        }
    }
    void OnCollisionEnter2D(Collision2D areaObject)
    {
        if (!areaObject.gameObject.CompareTag("Player") && !areaObject.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            Debug.Log($"{gameObject.gameObject.name.ToString()} has hit the collision {areaObject.gameObject.name.ToString()}!");
        }
        if (areaObject.gameObject.CompareTag("Enemy")) // A enemy has been hit
        {
            GameObject enemy = areaObject.gameObject;
            enemy.GetComponent<Rous_Soldier>().HitByBullet(20); // value is amount of damage to be applied, Im not really sure how to determine what that value will be so its 20 for now
        }
    }
    void OnTriggerEnter2D(Collider2D areaObject)
    {
        //Check if it's the player, update the game in the future to not only look for the player entity, for example: bullets can hit other bullets!
        if (!areaObject.CompareTag("Player") && !areaObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            Debug.Log($"{gameObject.gameObject.name.ToString()} has hit the trigger {areaObject.gameObject.name.ToString()}!");
        }
        else
        {
            Debug.Log($"{gameObject.name.ToString()} Hit the player");
        }

    }
    void FixedUpdate()
    {
        //Get the direction and magnitude
        physAbsVelocity = Mathf.Sqrt(Mathf.Pow(selfRigidBody.velocity.x, 2) + Mathf.Pow(selfRigidBody.velocity.y, 2));
        physDirection = Mathf.Atan2(selfRigidBody.velocity.y, selfRigidBody.velocity.x);
        
        //Set angle to direction
        selfRigidBody.SetRotation(physDirection * 180 / Mathf.PI);
        selfRigidBody.angularVelocity = 0;


        selfSpriteRenderer.size = new Vector2(tracerSize * 4 + physAbsVelocity * Time.deltaTime, tracerSize);

        lifeTime++;
        if (lifeTime >= 255)
        {
            Destroy(gameObject);
            //Kill after about 4.2 seconds
        }

        //TODO: Add an artificial drag
        //selfRigidBody.velocity = new Vector2(physAbs);


    }

}

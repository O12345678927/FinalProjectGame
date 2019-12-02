using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float dragCoef = 0.97f;
    public float tracerSize = 0.125f;
    public float initialDamage = 10;

    const float TURBULENCE_COEF = 0.5f;

    Rigidbody2D selfRigidBody;
    SpriteRenderer selfSpriteRenderer;

    private float physAbsVelocity;
    private float physDirection;
    private bool useTurbulence = false;
    private float turbulenceRate;
    private float turbulenceVelocity;

    private ushort lifeTime;

    // Start is called before the first frame update
    // use ignorecollision and tags/labels!!!!!!
    public void Initialize(float[] bulletData)
    {
        initialDamage = bulletData[1];
        dragCoef = bulletData[2];
    }
    public void UseTurbulence(bool condition, float rate)
    {
        useTurbulence = condition;
        turbulenceRate = rate*TURBULENCE_COEF;
        turbulenceVelocity = 0;
    }
    void Start()
    {
        if ((GetComponent(typeof(BoxCollider2D)) == null) || (GetComponent(typeof(Rigidbody2D)) == null)) {
            Debug.Log($"Bullet without box collider or rigid body...\nRemoving object {gameObject.name.ToString()}");
            Destroy(gameObject);
        }
        else
        {
            selfRigidBody = GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().size = new Vector2(tracerSize, tracerSize);

            //If the sprite renderer doesn't exist then I don't even care anymore, I'm gonna assign this without checking anyways
            selfSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        foreach (GameObject curObject in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            //Ignore physics for each
            //THIS IS LAGGY! find another way to do collision like this
            //Try IgnoreLayerCollision later!
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
        if (areaObject.gameObject.CompareTag("Rous_Soldier")) // An Enemy with script RousSoldier has been hit
        {
            GameObject enemy = areaObject.gameObject;
            enemy.GetComponent<Rous_Soldier>().HitByBullet(initialDamage);
        }
        if (areaObject.gameObject.CompareTag("Fauna")) // An enemy with script Fauna has been hit
        {
            GameObject enemy = areaObject.gameObject;
            enemy.GetComponent<Fauna>().HitByBullet(initialDamage);
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

        if (!useTurbulence)
        {
            physDirection = Mathf.Atan2(selfRigidBody.velocity.y, selfRigidBody.velocity.x);
            selfRigidBody.velocity = selfRigidBody.velocity * dragCoef;
        }
        else
        {
            turbulenceVelocity += turbulenceRate;
            physDirection = Mathf.Atan2(selfRigidBody.velocity.y, selfRigidBody.velocity.x)+ turbulenceVelocity;
            selfRigidBody.velocity = new Vector2(Mathf.Cos(physDirection) * physAbsVelocity * dragCoef, Mathf.Sin(physDirection) * physAbsVelocity * dragCoef);
        }

        //Set angle to direction
        selfRigidBody.SetRotation(physDirection * 180 / Mathf.PI);
        selfRigidBody.angularVelocity = 0;

        //Set size of sprite (Will be changed with the addition of a tracer effect child)
        selfSpriteRenderer.size = new Vector2(tracerSize + (physAbsVelocity * Time.deltaTime), tracerSize);

        lifeTime++;
        if (lifeTime >= 255)
        {
            Destroy(gameObject);
            //Kill after about 4 to 5 seconds
        }
        else if (physAbsVelocity < 1)
        {
            Destroy(gameObject);
            //Kill if too slow
        }
    }

}

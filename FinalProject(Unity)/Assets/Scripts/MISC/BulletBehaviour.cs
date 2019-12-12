using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float dragCoef = 0.97f;
    public float tracerSize = 0.2f;
    public int fadeMomentum = 8;
    public float fadeRatio = 0.5f;
    public GameObject tracerObject;

    const float TURBULENCE_COEF = 1.5f;

    Rigidbody2D selfRigidBody;
    SpriteRenderer selfSpriteRenderer;
    SpriteRenderer tracerSpriteRenderer;

    private float physAbsVelocity;
    private float physDirection;
    private bool useTurbulence = false;

    private float turbulenceRate;
    private float turbulenceVelocity;
    private float initialVelocity;
    private float initialDamage;
    private ushort lifeTime;

    // Start is called before the first frame update
    // use ignorecollision and tags/labels!!!!!!
    public void Initialize(float[] bulletData)
    {
        initialVelocity = bulletData[0];
        initialDamage = bulletData[1];
        dragCoef = bulletData[2];
    }
    public void SetProjectileSize(float newSize)
    {
        tracerSize = newSize;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(newSize, newSize);
        //GetComponent<BoxCollider2D>().size = new Vector2(2 * newSize, newSize);
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
            selfSpriteRenderer = GetComponent<SpriteRenderer>();
            tracerSpriteRenderer = tracerObject.GetComponent<SpriteRenderer>();
            selfRigidBody = GetComponent<Rigidbody2D>();

            GetComponent<BoxCollider2D>().size = new Vector2(2 * tracerSize, tracerSize);

            selfSpriteRenderer.size = new Vector2(tracerSize, tracerSize);
        }
    }
    void OnCollisionEnter2D(Collision2D areaObject)
    {
        if (!areaObject.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            //Debug.Log($"{gameObject.gameObject.name.ToString()} has hit the collision {areaObject.gameObject.name.ToString()}!");
        }
        if (areaObject.gameObject.CompareTag("Enemy")) // An Enemy has been hit
        {
            GameObject enemy = areaObject.gameObject;
            if (!(enemy.GetComponent(typeof(Rous_Soldier)) == null))
            {
                enemy.GetComponent<Rous_Soldier>().HitByBullet(initialDamage * Mathf.Sqrt(physAbsVelocity / initialVelocity));                
            }
            else if (!(enemy.GetComponent(typeof(Fauna)) == null))
            {
                enemy.GetComponent<Fauna>().HitByBullet(initialDamage * Mathf.Sqrt(physAbsVelocity / initialVelocity));                
            }
        }        
    }
    void OnTriggerEnter2D(Collider2D areaObject)
    {
        //Check if it's the player, update the game in the future to not only look for the player entity, for example: bullets can hit other bullets!
        if (!areaObject.CompareTag("Player") && !areaObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);           
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
        selfRigidBody.SetRotation(physDirection * Mathf.Rad2Deg);

        //Set size and position of tracer
        tracerSpriteRenderer.size = new Vector2((physAbsVelocity * Time.deltaTime) / 1.5f, tracerSize / 1.5f);
        tracerObject.transform.localPosition = new Vector2(-tracerSpriteRenderer.size.x / 2, 0);

        //Fade out the sprites
        if (physAbsVelocity < fadeMomentum)
        {
            selfSpriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Max(0, physAbsVelocity / fadeMomentum - fadeRatio));
            tracerSpriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Max(0, physAbsVelocity / fadeMomentum - fadeRatio));
        }
        lifeTime++;
        if (lifeTime >= 255)
        {
            Destroy(gameObject);
            //Kill after about 4 to 5 seconds
        }
        else if (physAbsVelocity < fadeMomentum*fadeRatio)
        {
            Destroy(gameObject);
            //Kill if too slow
        }
    }
}

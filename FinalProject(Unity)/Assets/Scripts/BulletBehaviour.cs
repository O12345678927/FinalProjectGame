using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    const float DRAG_COEF = 0.97f;
    public float tracerSize = 0.10f;
    Rigidbody2D selfRigidBody;
    SpriteRenderer selfSpriteRenderer;
    float physAbsVelocity;
    float physDirection;
    // Start is called before the first frame update
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
    }
    void OnCollisionEnter2D(Collision2D areaObject)
    {
        Destroy(gameObject);
        Debug.Log($"{gameObject.gameObject.name.ToString()} has hit the collision {areaObject.gameObject.name.ToString()}!");
    }
    void OnTriggerEnter2D(Collider2D areaObject)
    {
        //Check if it's the player, update the game in the future to not only look for the player entity, for example: bullets can hit other bullets!
        if (areaObject.name.ToString() != "Player")
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

        selfSpriteRenderer.size = new Vector2(tracerSize + physAbsVelocity * 0.1f, tracerSize);

        //TODO: Add an artificial drag
        //selfRigidBody.velocity = new Vector2(physAbs);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    private bool collisionDetected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(gameObject);
        collisionDetected = true;
    }

    public bool IsCollisionDetected()
    {
        return collisionDetected;
    }
}

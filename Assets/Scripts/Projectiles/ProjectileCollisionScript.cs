using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    private bool collisionDetected = false;
    private bool pierceEnemies = false;

    private void Start()
    {
        pierceEnemies = PlayerScript.MyInstance.IsProjectilePiercing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && (!pierceEnemies || pierceEnemies && !other.CompareTag("Enemy")))
            collisionDetected = true;
    }

    public bool IsCollisionDetected()
    {
        return collisionDetected;
    }
}

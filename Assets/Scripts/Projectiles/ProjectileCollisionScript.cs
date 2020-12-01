using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    private bool collisionDetected = false;
    private bool pierceEnemies = false;

    private bool enemyTouched = false;

    private void Start()
    {
        pierceEnemies = PlayerScript.MyInstance.IsProjectilePiercing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (!pierceEnemies || pierceEnemies && !other.CompareTag("Enemy"))
                collisionDetected = true;

            if (other.CompareTag("Enemy"))
            {
                enemyTouched = true;
                PlayerScript.MyInstance.MySuccessiveHit++;

                PlayerScript.MyInstance.HealOnSuccessiveHit();
            }
        }
    }

    public bool IsCollisionDetected()
    {
        return collisionDetected;
    }

    private void OnDestroy()
    {
        if (!enemyTouched)
            PlayerScript.MyInstance.MySuccessiveHit = 0;
    }
}

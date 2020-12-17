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
        //check if the player has piercing projectiles
        pierceEnemies = PlayerScript.MyInstance.IsProjectilePiercing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //projectile doesn't explodes when colliding with triggers
        if (!other.isTrigger)
        {
            //projectile explodes if it can't pierce enemies or if it can but is not colliding with an enemy
            if (!pierceEnemies || pierceEnemies && !other.CompareTag("Enemy"))
                collisionDetected = true;

            //when colliding with an enemy, keep track of it for various items
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
        //reset the number of successive hit from the players if no enemy has been touched by this projectile
        if (!enemyTouched)
            PlayerScript.MyInstance.MySuccessiveHit = 0;
    }
}

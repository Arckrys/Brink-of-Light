using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectiles : EnnemiesProjectileScript
{
    public int bounceTimes;
    private int bounceCounter;
    private Vector2 direction;

    protected override void Start()
    {
        bounceCounter = 0;
        base.Start();
        direction = new Vector2(xDirection, yDirection);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Spell") || other.gameObject.tag.Equals("Projectile")))
        {
            
            if (other.gameObject.tag.Equals("Player"))
            {
                collisionDetected = true;
                other.GetComponent<PlayerScript>().ReceiveDmgFromProjectile(MyBaseDamage);
            }
            else
            {
                if (bounceCounter == bounceTimes)
                {
                    collisionDetected = true;
                }
                else
                {
                    Vector2 collision =  other.ClosestPoint(transform.position);
                    Vector2 normal = ((Vector2)transform.position - collision).normalized;
                    direction = Vector2.Reflect(direction, normal);
                    xDirection = direction.x;
                    yDirection = direction.y;
                    bounceCounter++;
                }
            }
        }


    }
}

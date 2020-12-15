using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossBouncingProjectileScript : BouncingProjectiles
{

    public GameObject combustibleGameObject;
    public int combustibleSpawnRate;

    protected override void bounce(Collider2D other)
    {
        if (bounceCounter == bounceTimes)
        {
            collisionDetected = true;
            int rand = Random.Range(0, 91);
            if (rand < combustibleSpawnRate)
            {
                //if the projectile is on the bottom half, spawn the combustible toward the top, else toward the bot
                float combustibleOffsetY = 0.05f;
                if (transform.position.y > 0)
                    combustibleOffsetY = -combustibleOffsetY;

                //same for the x axis
                float combustibleOffsetX = 0.05f;
                if (transform.position.x > 0)
                    combustibleOffsetX = -combustibleOffsetX;

                GameObject combustible = Instantiate(combustibleGameObject, new Vector2(transform.position.x + combustibleOffsetX, transform.position.y + combustibleOffsetY), Quaternion.identity);
                combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
            }
        }
        else
        {
            Vector2 collision = other.ClosestPoint(transform.position);
            Vector2 normal = ((Vector2)transform.position - collision).normalized;
            direction = Vector2.Reflect(direction, normal);
            xDirection = direction.x;
            yDirection = direction.y;
            bounceCounter++;
        }
    }

}
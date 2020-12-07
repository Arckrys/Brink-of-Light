using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GlacePhenixProjectileScript : EnnemiesProjectileScript
{

    public GameObject combustibleGameObject;
    public int combustibleSpawnRate;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Spell") || other.gameObject.tag.Equals("Projectile")))
        {
            collisionDetected = true;
            if (other.gameObject.tag.Equals("Player"))
            {
                other.GetComponent<PlayerScript>().ReceiveDmgFromProjectile(MyBaseDamage);
            }
            else
            {
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
        }
    }

}
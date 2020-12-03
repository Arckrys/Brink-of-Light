using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class BatProjectileScript : EnnemiesProjectileScript
{
    
    private int destructionCounter;

    [SerializeField] private int destructionTime;


    // Start is called before the first frame update
    protected override void Start()
    {
        destructionCounter = 0;

        base.Start();

    }


    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (collisionDetected && rigidbody)
        {
            if (destructionCounter < destructionTime) destructionCounter++;
            else
            {
                Destroy(gameObject);
            }
            
        }

        if (!collisionDetected)
        {
            //update position variables
            x += xDirection * Time.deltaTime * projectileSpeed;
            y += yDirection * Time.deltaTime * projectileSpeed;

            //update physical position
            transform.localPosition = new Vector3(x, y, initialZ);

            timeTemp += Time.deltaTime;


            //destroy projectile if out of screen
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
                Destroy(gameObject);
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Spell") || other.gameObject.tag.Equals("Projectile")))
        {
            
            if (other.gameObject.tag.Equals("Player"))
            {
                other.GetComponent<PlayerScript>().ReceiveDmgFromProjectile(MyBaseDamage);
            }
            else
            {
                collisionDetected = true;
            }
        }
        
        
    }


}
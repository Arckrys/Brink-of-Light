using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class EnnemiesProjectileScript : MonoBehaviour
{
    private float x, y, spriteScaleX, spriteScaleY, timeTemp = 0;
    private float xDirection, yDirection;

    public float projectileSpeed = 10f;


    private float initialZ;

    private float projectileBaseDamage = 1f;
    private float projectileKnockback = 2f;
    private float projectileCritChance = 5f;

    private bool collisionDetected;

    private Light2D projectileLight;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D collider;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audio;

    [SerializeField] private AudioClip impactClip;

    public bool isCrit;

    // Start is called before the first frame update
    void Start()
    {
        collisionDetected = false;

        rigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        collider = GetComponent<PolygonCollider2D>() as PolygonCollider2D;

        audio = GetComponent<AudioSource>();

        projectileLight = GetComponent<Light2D>();

        initialZ = transform.position.z;


        UpdatePolygonCollider();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (collisionDetected && rigidbody)
        {
            
            //remove the projectile collisions and trigger the blast animation
            animator.SetBool("CollisionDetected", true);
            Destroy(rigidbody);
            Destroy(collider);
            //play the blast audio clip
            
            var mixer = Resources.Load("Sounds/AudioMixer") as AudioMixer;
            var volumeValue = .5f;
            var volume = !(mixer is null) && mixer.GetFloat("Volume", out volumeValue);

            if (volume)
            {
                audio.volume = 1-Math.Abs(volumeValue)/80;
            }
            
            audio.clip = impactClip;
            audio.Play();
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

    public void SetDirection(Vector2 direction, float initialX, float initialY)
    {
        x = initialX;
        y = initialY;

        direction = direction.normalized;

        xDirection = direction.x;
        yDirection = direction.y;


        //change the projectile's rotation to match the direction
        Vector2 baseVector = new Vector2(0, 1);
        float directionAngle = Vector2.Angle(baseVector, direction);

        if (xDirection > 0)
            directionAngle = -directionAngle;

        transform.Rotate(Vector3.forward * (directionAngle + 180));
    }

    public float MyBaseDamage
    {
        get
        {
            return projectileBaseDamage;
        }

        set
        {
            if (value < 1)
                projectileBaseDamage = 1;

            else
                projectileBaseDamage = value;

        }
    }


    public float MyKnockback
    {
        get
        {
            return projectileKnockback;
        }

        set
        {
            if (value < 2)
                projectileKnockback = 2;

            else
                projectileKnockback = value;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Spell") || other.gameObject.tag.Equals("Projectile")))
        {
            collisionDetected = true;
            if (other.gameObject.tag.Equals("Player"))
            {
                other.GetComponent<PlayerScript>().ReceiveDmgFromProjectile(MyBaseDamage);
            }
        }
        
        
    }


    private void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.autoTiling = true;
        collider.isTrigger = true;
    }
    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    private float x, y, spriteScaleX, spriteScaleY, timeTemp = 0;
    private float xDirection, yDirection;

    public float projectileShrinkSpeed = 0.05f;
    public float projectileSpeed = 10f;
    public float projectileShrinkFrequency = 0.05f;

    //variable used to change range
    public float projectileShrinkAcceleration = 1.1f;

    private float initialZ;

    private float projectileDamage = 1f;
    private float projectileKnockback = 2f;
    private float projectileCritChance = 5f;

    //ratio used to shrink the projectile into a square
    private float heightWidthRatio;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audio;

    [SerializeField] private AudioClip impactClip;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        boxCollider = GetComponent<BoxCollider2D>() as BoxCollider2D;

        audio = GetComponent<AudioSource>();

        initialZ = transform.position.z;

        float hitboxX, hitboxY;
        float hitboxYReduction = 0.75f;

        //change hitbox size
        hitboxX = spriteRenderer.bounds.size.x / transform.localScale.x;
        hitboxY = hitboxYReduction * spriteRenderer.bounds.size.y / transform.localScale.y;
        Vector3 newSize = new Vector3(hitboxX, hitboxY, 0);
        boxCollider.size = newSize;
        
        //change hitbox center
        boxCollider.offset = new Vector2(0, (hitboxYReduction - 1) * spriteRenderer.bounds.size.y / transform.localScale.y);

        heightWidthRatio = spriteRenderer.bounds.size.y / spriteRenderer.bounds.size.x;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<ProjectileCollisionScript>().IsCollisionDetected() && rigidbody)
        {
            //set the fireball as a square since explosion uses square sprites
            float explosionScale = 0.7f;
            transform.localScale = new Vector3(transform.localScale.x * explosionScale, transform.localScale.x * explosionScale, 0); ;

            //remove the projectile collisions and trigger the blast animation
            animator.SetBool("CollisionDetected", true);            
            Destroy(rigidbody);
            Destroy(boxCollider);

            //play the blast audio clip
            audio.clip = impactClip;
            audio.volume /= 2;
            audio.Play();
        }

        else if (!GetComponent<ProjectileCollisionScript>().IsCollisionDetected())
        {
            //update position variables
            x += xDirection * Time.deltaTime * projectileSpeed;
            y += yDirection * Time.deltaTime * projectileSpeed;

            //update physical position
            transform.localPosition = new Vector3(x, y, initialZ);

            timeTemp += Time.deltaTime;

            if (timeTemp > projectileShrinkFrequency)
            {
                spriteScaleX = transform.localScale.x;
                spriteScaleY = transform.localScale.y;

                if (spriteScaleX > 0.1 && spriteScaleY > 0.1)
                {
                    float newX, newY;
                    newX = spriteScaleX - projectileShrinkSpeed < 0 ? 0.01f : spriteScaleX - projectileShrinkSpeed;
                    newY = spriteScaleY - projectileShrinkSpeed * heightWidthRatio < 0 ? 0.01f : spriteScaleX - projectileShrinkSpeed * heightWidthRatio;

                    Vector3 newScale = new Vector3(newX, newY, 0);
                    transform.localScale = newScale;

                    timeTemp = 0;
                    projectileShrinkSpeed *= projectileShrinkAcceleration;
                }

                else
                {
                    Destroy(gameObject);
                }
            }

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

    public float MyDamage
    {
        get
        {
            return projectileDamage;
        }

        set
        {
            if (value < 0)
                projectileDamage = 1;

            else
                projectileDamage = value;
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
            if (value < 0)
                projectileKnockback = 2;

            else
                projectileKnockback = value;
        }
    }

    public float MyRange
    {
        get
        {
            return projectileShrinkAcceleration;
        }

        set
        {
            if (value < 0)
                projectileShrinkAcceleration = 1.1f;

            else
                projectileShrinkAcceleration = value;
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

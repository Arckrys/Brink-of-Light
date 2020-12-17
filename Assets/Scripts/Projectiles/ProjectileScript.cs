using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    private float x, y, spriteScaleX, spriteScaleY, timeTemp = 0;
    private float xDirection, yDirection;

    public float projectileShrinkSpeed = 0.05f;
    public float projectileSpeed = 10f;
    public float projectileShrinkFrequency = 0.1f;

    //variable used to change range, making the projectile shrink faster as it gets smaller
    public float projectileShrinkAcceleration = 1.1f;

    private float initialZ;

    private float projectileBaseDamage = 1f;
    private float projectileCurrentDamage;
    private float projectileKnockback = 2f;
    private float projectileCritChance = 5f;
    private float bossBonusDamage = 0;
    private bool isBurning = false;

    private float heightWidthRatio;

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
        if (spriteRenderer is null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        rigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        collider = GetComponent<PolygonCollider2D>() as PolygonCollider2D;

        audio = GetComponent<AudioSource>();

        projectileLight = GetComponent<Light2D>();

        initialZ = transform.position.z;        

        UpdatePolygonCollider();

        //allow the projectile to shrink and update its current damage right after its creation
        timeTemp = projectileShrinkFrequency;

        heightWidthRatio = spriteRenderer.bounds.size.y / spriteRenderer.bounds.size.x;
    }


    void FixedUpdate()
    {
        //when the projectile has collided with an object its supposed to collide with and still has a rigidbody
        if (GetComponent<ProjectileCollisionScript>().IsCollisionDetected() && rigidbody)
        {
            float explosionScale = 0.7f;
            transform.localScale = new Vector3(transform.localScale.x * explosionScale, transform.localScale.x * explosionScale, 0);

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
                audio.volume = 0.1f-Math.Abs(volumeValue)/80;
            }
            
            audio.clip = impactClip;
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

            //when the projectile has to shrink
            if (timeTemp > projectileShrinkFrequency)
            {
                spriteScaleX = transform.localScale.x;
                spriteScaleY = transform.localScale.y;

                //if projectile isn't too small
                if (spriteScaleX > 0.1 && spriteScaleY > 0.1)
                {
                    //reduce the scale of the projectile by projectileShrinkSpeed
                    float newX, newY;
                    newX = spriteScaleX - projectileShrinkSpeed < 0 ? 0.01f : spriteScaleX - projectileShrinkSpeed;
                    newY = spriteScaleY - projectileShrinkSpeed * heightWidthRatio < 0 ? 
                        0.01f : spriteScaleX - projectileShrinkSpeed * heightWidthRatio;

                    Vector3 newScale = new Vector3(newX, newY, 0);
                    transform.localScale = newScale;
                    timeTemp = 0;

                    //update the projectile's light
                    if(newX < 1.5 && newY < 1.5)
                    {
                        projectileLight.pointLightOuterRadius = System.Math.Max(0, projectileLight.pointLightOuterRadius - 0.2f);
                    }

                    //increase the shrink speed
                    projectileShrinkSpeed *= projectileShrinkAcceleration;

                    //damage multiplier based on projectile scale (-0.8f is arbitrary)
                    float damageMultiplier = newX + newY / 2 - 0.8f;
                    //round damage to 0.5
                    float newDamage = Mathf.Round(damageMultiplier * projectileBaseDamage * 2) / 2;

                    //projectile distance damage multiplier is forced between 0.5 and 2 times the base damage
                    if (newDamage < projectileBaseDamage / 2)
                        newDamage = projectileBaseDamage / 2;
                    else if (newDamage > projectileBaseDamage * 2)
                        newDamage = projectileBaseDamage * 2;

                    projectileCurrentDamage = newDamage;
                }

                //destroy projectile when it becomes too small
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

    //set the direction of the projectile based on it's target point
    public void SetDirection(Vector2 direction, float initialX, float initialY)
    {
        //update position
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

            MyCurrentDamage = projectileBaseDamage;
        }
    }

    public float MyCurrentDamage
    {
        get
        {
            return projectileCurrentDamage;
        }

        set
        {
            projectileCurrentDamage = value;
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

    public float MyRange
    {
        get
        {
            return projectileShrinkSpeed;
        }

        set
        {
            if (value < 0.005f)
                projectileShrinkSpeed = 0.005f;

            else
                projectileShrinkSpeed = value;
        }
    }

    public float MyBossBonusDamage
    {
        get
        {
            return bossBonusDamage;
        }

        set
        {
            bossBonusDamage = value;
        }
    }

    public bool IsBurning
    {
        get
        {
            return isBurning;
        }

        set
        {
            isBurning = value;
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

    public void SetColor(Color newColor)
    {
        if (spriteRenderer is null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        spriteRenderer.color = newColor;
    }
}

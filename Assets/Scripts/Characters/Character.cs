﻿using UnityEngine;
using System.Collections;
using System.Globalization;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public StatUI life;

    [SerializeField] public float initLife;

    [SerializeField] public StatField attack;

    [SerializeField] public float initAttack;

    [SerializeField] public StatField movementSpeed;

    [SerializeField] public float initMovementSpeed;

    [SerializeField] public StatField critChance;

    [SerializeField] public float initCritChance;
    
    [SerializeField] public StatField critDamage;

    [SerializeField] public float initCritDamage;

    [SerializeField] public StatField range;

    [SerializeField] public float initRange;

    [SerializeField] public StatField attackSpeed;

    [SerializeField] public float initAttackSpeed;
    
    [SerializeField] public StatField knockback;

    [SerializeField] public float initKnockback;

    private Rigidbody2D myRigidbody;

    protected Vector2 direction;

    private int isChangingDirection;

    protected bool IsMoving => direction.x != 0 || direction.y != 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        // Initialize player current stats based on their initial values
        life.Initialized(initLife, initLife);
        InitStatField(ref attack, initAttack, false);
        InitStatField(ref movementSpeed, initMovementSpeed, false);
        InitStatField(ref critChance, initCritChance, false);
        InitStatField(ref critDamage, initCritDamage, false);
        InitStatField(ref range, initRange, false);
        InitStatField(ref attackSpeed, initAttackSpeed, false);
        InitStatField(ref knockback, initKnockback, false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Update character's hitbox when changing direction
        if (isChangingDirection > 0)
        {
            UpdatePolygonCollider();
            isChangingDirection -= 1;
        }
    }

    // FixedUpdate is called at a constant rate, regardless of the user's machine
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void InitStatField(ref StatField stat, float initValue, bool variable)
    {
        // Add missing component if variable is null
        if (!stat)
        {
            stat = gameObject.AddComponent<StatField>();
        }
        
        // Initiate character stat
        stat.Initialized(initValue, initValue, false);
    }

    // Moves the character around according to its direction and speed attributes
    protected void Move()
    {
        myRigidbody.velocity = direction.normalized * movementSpeed.MyCurrentValue;
    }

    // Recreate character collider
    private void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
    
    private void ResetAnimator(Animator animator)
    {
        animator.SetBool("FacingRight", false);
        animator.SetBool("FacingLeft", false);
        animator.SetBool("FacingDown", false);
        animator.SetBool("FacingUp", false);
        animator.SetBool("Idle", false);
    }

    // Plays the proper animation given a direction
    protected void FaceDirection(Vector2 newDir, Animator animator)
    {
        // When not moving
        if (newDir.x == 0 && newDir.y == 0)
        {
            if (!animator.GetBool("Idle"))
            {
                ResetAnimator(animator);
                animator.SetBool("Idle", true);
                isChangingDirection = 2;
            }
        }
        else
        {
            // Compute the angle to get the right animation (orientation) to play
            var angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
            
            if (angle < 40 && angle > -40)
            {
                if (!animator.GetBool("FacingRight"))
                {
                    ResetAnimator(animator);
                    animator.SetBool("FacingRight", true);
                    isChangingDirection = 2;
                }
                    
            }
            if (angle <= 135 && angle >= 45)
            {
                if (!animator.GetBool("FacingUp"))
                {
                    ResetAnimator(animator);
                    animator.SetBool("FacingUp", true);
                    isChangingDirection = 2;
                }
            }
            if (angle < -140 || angle > 140)
            {
                if (!animator.GetBool("FacingLeft"))
                {
                    ResetAnimator(animator);
                    animator.SetBool("FacingLeft", true);
                    isChangingDirection = 2;
                }
            }
            if (angle <= -45 && angle >= -135)
            {
                if (!animator.GetBool("FacingDown"))
                {
                    ResetAnimator(animator);
                    animator.SetBool("FacingDown", true);
                    isChangingDirection = 2;
                }
            }
        }
    }    
}

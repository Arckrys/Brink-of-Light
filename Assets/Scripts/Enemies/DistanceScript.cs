﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceScript : BasicEnemyController
{

    protected bool attacking;
    protected bool attackResting;
    public float attackFrequency;
    protected int dashCounter;
    protected int attackRestingCounter;

    protected override void Start()
    {
        dashCounter = 0;

        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (!CanvasTransitionScript.MyInstance.isDoingTransition)
        {
            Behaviour();
        }
        //print(movementSpeed.MyCurrentValue);
        base.FixedUpdate();
    }


    /// <summary>
    /// fonction permettant d'extraire l'action de tir du Behaviour afin de l'override plus facilement et de créer differents types d'ennemis
    /// </summary>
    /// <param name="direction">direction dans laquelle tir l'ennemi</param>
    protected virtual void shoot(Vector2 direction)
    {
        FireProjectileAtDirection(direction);
    }


    /// <summary>
    /// Fonction ayant pour but de stocker le comportement de l'ennemi
    /// </summary>
    protected void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")) && !attacking && !attackResting)
            {
                MoveToPlayer();
            }
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if(!(gfxAnim.GetBool("Knockback")) && !attacking && !attackResting)
                {

                    direction = (player.position - transform.position)*0;
                    Move();
                    FaceDirection(player.position - transform.position, gfxAnim);
                    gfxAnim.SetBool("Attacking", true);
                    shoot(player.position - transform.position);
                    attackResting = true;
                    attackRestingCounter = (int)((1 / attackFrequency) * 10);
                    gameObject.layer = 13;

                }
            }
            if (attackResting)
            {
                if (attackRestingCounter == 0)
                {
                    Move();
                    attackResting = false;
                    gfxAnim.SetBool("Attacking", false);
                    gameObject.layer = 10;
                }

                if (attackRestingCounter > 0)
                {
                    direction = (player.position - transform.position) * 0;
                    Move();
                    attackRestingCounter -= 1;
                }
            }
        }
        else
        {
            Wander();
        }

        if (gfxAnim.GetBool("Knockback") && attacking)
        {
            knockbackTimer += 1;
            Move();
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }

        if (gfxAnim.GetBool("Knockback") && !attacking)
        {
            knockbackTimer += 1;
            direction = -1 * (player.position - transform.position);
            Move();
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }
    }
}

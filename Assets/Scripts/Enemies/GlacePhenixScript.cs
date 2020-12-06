using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacePhenixScript : BasicEnemyController
{

    private bool dashing;
    private bool dashResting;
    public int dashDistance;
    public int dashSpeedMultiplicator;
    public int dashRestingTime;
    private int dashCounter;
    private int dashRestingCounter;
    private int attackNumber;
    private int phase;
    private bool rampage;
    public float attackFrequency;
    protected int attackRestingCounter;

    protected override void Start()
    {
        dashCounter = 0;
        attackNumber = 0;
        phase = 1;
        rampage = false;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (!CanvasTransitionScript.MyInstance.isDoingTransition)
        {
            Behaviour();
        }
        base.FixedUpdate();
    }


    //Verification des collision pour arrêter le sprint en cas de collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Spell") && !collision.gameObject.tag.Equals("Enemy"))
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                direction = -1 * direction;
            }
            else
            {
                dashCounter = 0;
            }
            
        }
    }



    /// <summary>
    /// Fonction ayant pour but de stocker le comportement de l'ennemi
    /// </summary>
    private void Behaviour()
    {
        if(!rampage && life.MyCurrentValue < 100)
        {
            rampage = true;
            movementSpeed.MyMaxValue *= 1.2f;
            attackFrequency *= 1.5f;

        }
        if (playerDetected)
        {
            //rapprochement vers le joueur
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")) && !dashing)
            {
                MoveToPlayer();
            }


            //phase de dash
            if(phase == 1)
            {
                if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    if (!(gfxAnim.GetBool("Knockback")) && !dashing && !dashResting)
                    {

                        movementSpeed.MyMaxValue *= dashSpeedMultiplicator;
                        direction = player.position - transform.position;
                        Move();
                        FaceDirection(direction, gfxAnim);
                        dashCounter = dashDistance;
                        dashing = true;
                        gfxAnim.SetBool("Dashing", true);
                        gameObject.layer = 13;
                    }
                }
                if (dashing)
                {
                    if (dashCounter == 0)
                    {
                        gfxAnim.SetBool("Dashing", false);
                        dashing = false;
                        gameObject.layer = 10;
                        dashResting = true;
                        dashRestingCounter = dashRestingTime;
                        movementSpeed.MyMaxValue /= dashSpeedMultiplicator;
                        direction = (player.position - transform.position) * 0;
                        Move();
                    }

                    if (dashCounter > 0)
                    {
                        Move();
                        dashCounter -= 1;
                    }
                }
                if (dashResting)
                {
                    

                    if (dashRestingCounter == 0 && attackNumber != 3)
                    {
                        Move();
                        dashResting = false;
                        attackNumber++;
                    }

                    if (dashRestingCounter == 0 && attackNumber == 3)
                    {
                        Move();
                        dashResting = false;
                        phase = 2;
                        attackNumber = 0;
                    }

                    if (dashRestingCounter > 0)
                    {
                        direction = (player.position - transform.position) * 0;
                        Move();
                        dashRestingCounter -= 1;
                    }
                }
            }

            //phase à distance
            if(phase == 2)
            {
                if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    if (!(gfxAnim.GetBool("Knockback")) && !dashing && !dashResting)
                    {

                        direction = (player.position - transform.position) * 0;
                        Move();
                        FaceDirection(player.position - transform.position, gfxAnim);
                        gfxAnim.SetBool("Dashing", true);
                        shoot(player.position - transform.position);
                        dashResting = true;
                        attackRestingCounter = (int)((1 / attackFrequency) * 10);
                    }
                }
                if (dashResting)
                {
                    if (attackRestingCounter == 0 && attackNumber != 3)
                    {
                        Move();
                        dashResting = false;
                        gfxAnim.SetBool("Dashing", false);
                        attackNumber++;
                    }

                    if (attackRestingCounter == 0 && attackNumber == 3)
                    {
                        Move();
                        dashResting = false;
                        gfxAnim.SetBool("Dashing", false);
                        phase = 1;
                        attackNumber = 0;
                    }

                    

                    if (attackRestingCounter > 0)
                    {
                        direction = (player.position - transform.position) * 0;
                        Move();
                        attackRestingCounter -= 1;
                    }
                }
            }
            
        }
        else
        {
            Wander();
        }

        if (gfxAnim.GetBool("Knockback") && dashing)
        {
            knockbackTimer += 1;
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }

        if (gfxAnim.GetBool("Knockback") && !dashing)
        {
            knockbackTimer += 1;
            direction = 0 * (player.position - transform.position);
            Move();
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
                Move();
            }
        }
    }

    /// <summary>
    /// fonction permettant d'extraire l'action de tir du Behaviour afin de l'override plus facilement et de créer differents types d'ennemis
    /// </summary>
    /// <param name="direction">direction dans laquelle tir l'ennemi</param>
    protected virtual void shoot(Vector2 direction)
    {
        FireProjectileAtDirection(Rotate(direction, 30f));
        FireProjectileAtDirection(Rotate(direction, -30f));
        FireProjectileAtDirection(direction);
    }
}

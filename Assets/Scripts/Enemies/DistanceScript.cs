using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DistanceScript : BasicEnemyController
{

    private bool attacking;
    private bool attackResting;
    public float attackFrequency;
    private int dashCounter;
    private int attackRestingCounter;

    protected override void Start()
    {
        dashCounter = 0;

        base.Start();
    }

    protected override void FixedUpdate()
    {
        Behaviour();
        //print(movementSpeed.MyCurrentValue);
        base.FixedUpdate();
    }

    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")) && !attacking && !attackResting)
            {
                direction = player.position - transform.position;
                Vector2 facingDirection = player.position - transform.position;
                FaceDirection(facingDirection, gfxAnim);
            }
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if(!(gfxAnim.GetBool("Knockback")) && !attacking && !attackResting)
                {

                    direction = (player.position - transform.position)*0;
                    FaceDirection(player.position - transform.position, gfxAnim);
                    gfxAnim.SetBool("Attacking", true);
                    FireProjectileAtDirection(player.position - transform.position);
                    attackResting = true;
                    attackRestingCounter = (int)((1 / attackFrequency) * 10);

                }
            }
            if (attackResting)
            {
                if (attackRestingCounter == 0)
                {
                    attackResting = false;
                    gfxAnim.SetBool("Attacking", false);
                }

                if (attackRestingCounter > 0)
                {
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
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }

        if (gfxAnim.GetBool("Knockback") && !attacking)
        {
            knockbackTimer += 1;
            direction = -1 * (player.position - transform.position);
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }
    }
}

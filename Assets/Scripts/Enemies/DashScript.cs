using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DashScript : BasicEnemyController
{

    private bool dashing;
    private bool dashResting;
    public int dashDistance;
    public int dashSpeedMultiplicator;
    public int dashRestingTime;
    private int dashCounter;
    private int dashRestingCounter;

    protected override void Start()
    {
        dashCounter = 0;

        base.Start();
    }

    protected override void FixedUpdate()
    {
        Behaviour();
        print(movementSpeed.MyCurrentValue);
        base.FixedUpdate();
    }

    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")) && !dashing)
            {
                direction = player.position - transform.position;
                Vector2 facingDirection = player.position - transform.position;
                FaceDirection(facingDirection, gfxAnim);
            }
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if(!(gfxAnim.GetBool("Knockback")) && !dashing && !dashResting)
                {

                    movementSpeed.MyMaxValue *= dashSpeedMultiplicator;
                    direction = player.position - transform.position;
                    dashCounter = dashDistance;
                    dashing = true;
                    gfxAnim.SetBool("Dashing", true);

                }
            }
            if (dashing)
            {
                if (dashCounter == 0)
                {
                    gfxAnim.SetBool("Dashing", false);
                    dashing = false;
                    dashResting = true;
                    dashRestingCounter = dashRestingTime;
                    movementSpeed.MyMaxValue /= dashSpeedMultiplicator;
                    direction = player.position - transform.position * 0;
                }

                if (dashCounter > 0)
                {
                    dashCounter -= 1;
                }
            }
            if (dashResting)
            {
                if (dashRestingCounter == 0)
                {
                    dashResting = false;
                }

                if (dashRestingCounter > 0)
                {
                    dashRestingCounter -= 1;
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
            direction = -1 * (player.position - transform.position);
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }
    }
}

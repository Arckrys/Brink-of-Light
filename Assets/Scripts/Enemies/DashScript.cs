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
            dashCounter = 0;
        }
    }




    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")) && !dashing)
            {
                MoveToPlayer();
            }
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if(!(gfxAnim.GetBool("Knockback")) && !dashing && !dashResting)
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
                    print(dashCounter);
                    Move();
                    dashCounter -= 1;
                }
            }
            if (dashResting)
            {
                if (dashRestingCounter == 0)
                {
                    Move();
                    dashResting = false;
                }

                if (dashRestingCounter > 0)
                {
                    print("dashresting");
                    direction = (player.position - transform.position) * 0;
                    Move();
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
            print(dashing);
            knockbackTimer += 1;
            direction = -1 * (player.position - transform.position);
            Move();
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
                Move();
            }
        }
    }
}

using UnityEngine;

class WolfScript : BasicEnemyController
{
    protected override void FixedUpdate()
    {
        Behaviour();

        base.FixedUpdate();
    }

    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")))
            {
                MoveToPlayer();
            }
        }
        else
        {
            Wander();
        }

        if (gfxAnim.GetBool("Knockback"))
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

using UnityEngine;

class WolfScript : BasicEnemyController
{
    protected override void FixedUpdate()
    {
        Behaviour();
        
        base.Update();
    }

    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")))
            {
                direction = player.position - transform.position;
                Vector2 facingDirection = player.position - transform.position;
                FaceDirection(facingDirection, gfxAnim);
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
            print(knockbackIntensity);
            if (knockbackTimer > knockbackIntensity - knockbackResistance)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }
    }
}

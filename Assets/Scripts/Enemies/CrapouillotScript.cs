using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrapouillotScript : DistanceScript
{
    protected override void shoot(Vector2 direction)
    {
        FireProjectileAtDirection(Rotate(direction,30f));
        FireProjectileAtDirection(Rotate(direction, -30f));
        FireProjectileAtDirection(direction);
    }
}

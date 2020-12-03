using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : DistanceScript
{
    protected override void shoot(Vector2 direction)
    {
        FireProjectileAtDirection(Rotate(direction,5f));
        FireProjectileAtDirection(Rotate(direction, -5f));
    }
}

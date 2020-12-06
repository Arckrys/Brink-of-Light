using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGhostScript : DistanceScript
{
    protected override void shoot(Vector2 direction)
    {
        FireProjectileAtDirection(Rotate(direction,90f));
        FireProjectileAtDirection(Rotate(direction, -90f));
        FireProjectileAtDirection(direction);
    }
}

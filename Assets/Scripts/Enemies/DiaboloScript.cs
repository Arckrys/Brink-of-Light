using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaboloScript : DistanceScript
{
    public int AOEDelay;
    private int AOEtimer;
    public int SpawnDelay;
    private int Spawntimer;
    public GameObject Bat;

    protected override void Start()
    {
        AOEtimer = 0;
        Spawntimer = 0;
        base.Start();
    }

    protected override void shoot(Vector2 direction)
    {

        FireProjectileAtDirection(direction);
        FireProjectileAtDirection(Rotate(direction, -30f));
        FireProjectileAtDirection(Rotate(direction, 30f));

    }

    protected override void FixedUpdate()
    {
        if (AOEtimer > AOEDelay)
        {
            Vector2 aim = player.position - transform.position;
            AOEtimer=0;
            FireProjectileAtDirection(aim);
            FireProjectileAtDirection(Rotate(aim, -30f));
            FireProjectileAtDirection(Rotate(aim, 30f));
            FireProjectileAtDirection(Rotate(aim, -60f));
            FireProjectileAtDirection(Rotate(aim, 60f));
            FireProjectileAtDirection(Rotate(aim, -90f));
            FireProjectileAtDirection(Rotate(aim, 90f));
            FireProjectileAtDirection(Rotate(aim, -120f));
            FireProjectileAtDirection(Rotate(aim, 120f));
            FireProjectileAtDirection(Rotate(aim, -150f));
            FireProjectileAtDirection(Rotate(aim, 150f));
            FireProjectileAtDirection(Rotate(aim, -180f));

        }
        if(Spawntimer > SpawnDelay)
        {
            Spawntimer = 0;
            GameObject combustible = Instantiate(Bat, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
        }
        AOEtimer++;
        Spawntimer++;
        base.FixedUpdate();
    }
}

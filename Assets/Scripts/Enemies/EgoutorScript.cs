using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoutorScript : GlacePhenixScript
{
    public GameObject Slime;
    private int spawnPhase;
    protected override void Start()
    {
        spawnPhase = 0;
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(this.life.MyCurrentValue < this.initLife * 0.75 && spawnPhase == 0)
        {
            spawnPhase++;
            GameObject combustible = Instantiate(Slime, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
        }
        if (this.life.MyCurrentValue < this.initLife * 0.5 && spawnPhase == 1)
        {
            spawnPhase++;
            GameObject combustible = Instantiate(Slime, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
        }
        if (this.life.MyCurrentValue < this.initLife * 0.25 && spawnPhase == 2)
        {
            spawnPhase++;
            GameObject combustible = Instantiate(Slime, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
        }
    }
}

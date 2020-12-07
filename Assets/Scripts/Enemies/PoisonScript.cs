using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonScript : MonoBehaviour
{
    public int damage;
    public int duration;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if(timer > duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("collide");
        if (other.gameObject.tag.Equals("Player"))
        {
            other.GetComponent<PlayerScript>().ReceiveDmgFromProjectile(damage);
        }
    }
}

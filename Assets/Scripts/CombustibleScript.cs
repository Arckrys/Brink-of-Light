using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustibleScript : MonoBehaviour
{
    private Animator combustibleAnimator;
    private BoxCollider2D collider;
    bool isLit = true;

    // Start is called before the first frame update
    void Start()
    {
        combustibleAnimator = GetComponentInChildren<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        combustibleAnimator.SetBool("isLit", isLit);
    }

    void OnTriggerEnter2D(Collider2D other) // Triggered when a rigidBody touches the collider
    {
        if (other.gameObject.name == "Player")
        {
            isLit = false;
            //Destroy(collider);
        }
        else if(other.gameObject.name == "ProjectileSprite(Clone)")
        {
            isLit = true;
            //Destroy(collider);
        }


    }

}

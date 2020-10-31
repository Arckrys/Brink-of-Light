﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustibleScript : MonoBehaviour
{
    private Animator combustibleAnimator;
    private BoxCollider2D collider;
    bool isLit = true;
    private float healingValue;

    // Start is called before the first frame update
    void Start()
    {
        combustibleAnimator = GetComponentInChildren<Animator>();
        collider = GetComponent<BoxCollider2D>();

        healingValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        combustibleAnimator.SetBool("isLit", isLit);
    }

    void OnTriggerEnter2D(Collider2D other) // Triggered when a rigidBody touches the collider
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isLit = false;
            PlayerScript.MyInstance.LifeCurrentValue += healingValue;
            //Destroy(collider);
        }
        else if(other.gameObject.tag.Equals("Spell"))
        {
            isLit = true;
            //Destroy(collider);
        }


    }

}
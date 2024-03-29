﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CombustibleScript : MonoBehaviour
{
    private Animator combustibleAnimator;
    private BoxCollider2D combustibleCollider;
    [SerializeField] bool isLit;
    private bool hasHealedPlayer;
    [SerializeField] private float healingValue = 25;
    [SerializeField] bool isDestroyedAfterFire;
    private bool hasAnimatorBurntParameter;

    // Start is called before the first frame update
    void Start()
    {
        hasHealedPlayer = false;
        combustibleAnimator = GetComponentInChildren<Animator>();
        combustibleCollider = GetComponent<BoxCollider2D>();
        hasAnimatorBurntParameter = HasParameter("isBurnt", combustibleAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        combustibleAnimator.SetBool("isLit", isLit);
        if (hasAnimatorBurntParameter)
            combustibleAnimator.SetBool("isBurnt", hasHealedPlayer);
    }

    void OnTriggerEnter2D(Collider2D other) // Triggered when a rigidBody touches the collider
    {
        if (other.gameObject.CompareTag("Player") && isLit)
        {
            hasHealedPlayer = true;
            isLit = false;
            PlayerScript.MyInstance.PlayerCurrentLife += healingValue;
            CombatTextManager.MyInstance.CreateText(PlayerScript.MyInstance.transform.position, healingValue.ToString(CultureInfo.InvariantCulture), DamageType.Heal, 1.0f, false);
            //Destroy(collider);

            if(isDestroyedAfterFire)
                Destroy(combustibleAnimator.GetComponent<PolygonCollider2D>());
        }
        
        if(other.gameObject.CompareTag("Spell") && !isLit && !hasHealedPlayer)
        {
            isLit = true;
            //Destroy(collider);
        }
    }

    public bool GetHasHealedPlayer()
    {
        return hasHealedPlayer;
    }

    public void SetHasHealedPlayer(bool b)
    {
        hasHealedPlayer = b;
    }

    public void SetIsLit(bool b)
    {
        isLit = b;
    }

    private static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}

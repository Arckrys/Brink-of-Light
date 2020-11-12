using System.Collections;
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
    private float healingValue;
    [SerializeField] bool isDestroyedAfterFire;

    // Start is called before the first frame update
    void Start()
    {
        hasHealedPlayer = false;
        combustibleAnimator = GetComponentInChildren<Animator>();
        combustibleCollider = GetComponent<BoxCollider2D>();

        healingValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        combustibleAnimator.SetBool("isLit", isLit);
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
                Destroy(GetComponent<PolygonCollider2D>());
        }
        else if(other.gameObject.CompareTag("Spell") && !isLit && !hasHealedPlayer)
        {
            isLit = true;
            //Destroy(collider);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character
{
    private Animator mouvementAnimator;

    private float timeSinceLastAttack = 0;
    public float nextAttackDelay = 0.1f;

    public GameObject Projectile;

    // Start is called before the first frame update
    protected override void Start()
    {
        mouvementAnimator = GetComponent<Animator>();
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        //lifeAnimator.SetFloat("life", life.MyCurrentValue / life.MyMaxValue);

        HandleLayers();

        base.Update();
    }

    private void UpdateLife()
    {
        
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.O))
        {
            life.MyCurrentValue -= 1;
        }
        if (Input.GetKey(KeyCode.I))
        {
            life.MyCurrentValue += 1;
        }

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }

        timeSinceLastAttack += Time.deltaTime;

        if (Input.GetMouseButton(0) && timeSinceLastAttack > nextAttackDelay)
        {
            FireProjectile();
            timeSinceLastAttack = 0;
        }
    }

    private void FireProjectile()
    {
        //get mouse position in world space
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //substract character position to change the origin of the projectile direction
        worldPosition = worldPosition - new Vector2(transform.position.x, transform.position.y);

        //create projectile
        GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileScript>().SetDirection(worldPosition, transform.position.x, transform.position.y);
    }

    private void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("Walk Layer");

            mouvementAnimator.SetFloat("X_speed", direction.x);
            mouvementAnimator.SetFloat("Y_speed", direction.y);
        }
        else
        {
            ActivateLayer("Idle Layer");
        }
    }

    private void ActivateLayer(string layerName)
    {
        for (int i = 0; i < mouvementAnimator.layerCount; i++)
        {
            mouvementAnimator.SetLayerWeight(i, 0);
        }

        mouvementAnimator.SetLayerWeight(mouvementAnimator.GetLayerIndex(layerName), 1);
    }
}

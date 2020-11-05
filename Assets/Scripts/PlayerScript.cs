using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character
{
    [SerializeField] private Stat LifeBar;
    
    private Animator mouvementAnimator;

    [SerializeField] private GameObject razakusMenu;
    private bool menuIsActive = false;

    private float timeSinceLastAttack = 0, timeSinceLastHit = 0, timeSinceLastAction = 0;

    private float invincibilityTime = 0.5f;

    public GameObject Projectile;
    private static PlayerScript instance;

    // Start is called before the first frame update
    protected override void Start()
    {
        mouvementAnimator = GetComponent<Animator>();
        razakusMenu.SetActive(menuIsActive);
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        HandleLayers();

        UpdateLifeImage();

        base.Update();

        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;
        timeSinceLastAction += Time.deltaTime;
    }

    public static PlayerScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerScript>();
            }
            return instance;
        }
    }

    public float Invincibility
    {
        get
        {
            return invincibilityTime;
        }

        set 
        {
            invincibilityTime = value;
        }
    }


    private void UpdateLifeImage()
    {
        Vector3 scale = life.transform.localScale;

        if (scale.x != life.MyCurrentValue / life.MyMaxValue && scale.y != life.MyCurrentValue / life.MyMaxValue)
        {
            scale.x = Mathf.Lerp(scale.x, life.MyCurrentValue / life.MyMaxValue, Time.deltaTime);
            scale.y = Mathf.Lerp(scale.y, life.MyCurrentValue / life.MyMaxValue, Time.deltaTime);

            life.transform.localScale = scale;
            
            LifeBar.GetComponent<Image>().fillAmount = life.MyCurrentValue / life.MyMaxValue;
        }
    }

    private void GetInput()
    {
        direction = Vector2.zero;

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
        

        if (Input.GetMouseButton(0) && timeSinceLastAttack > attackSpeed.MyCurrentValue)
        {
            FireProjectile();
            timeSinceLastAttack = 0;
        }

        if (Input.GetKey(KeyCode.E) && timeSinceLastAction > 0.2)
        {
            menuIsActive = !menuIsActive;
            timeSinceLastAction = 0;
            razakusMenu.SetActive(menuIsActive);
        }

    }

    private void FireProjectile()
    {
        //get mouse position in world space
        Vector2 screenPosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //substract character position to change the origin of the projectile direction
        worldPosition -= new Vector2(transform.position.x, transform.position.y);

        //create projectile
        GameObject projectile = Instantiate(Projectile, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        projectile.GetComponent<ProjectileScript>().SetDirection(worldPosition, transform.position.x, transform.position.y);
        projectile.GetComponent<ProjectileScript>().MyDamage = AttackMaxValue;
        projectile.GetComponent<ProjectileScript>().MyKnockback = KnockbackMaxValue;
        projectile.GetComponent<ProjectileScript>().MyRange = 1/RangeMaxValue;

        //player lose one health per shot
        life.MyCurrentValue -= 1;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && timeSinceLastHit > invincibilityTime)
        {
            timeSinceLastHit = 0;

            float damageReceived = collision.gameObject.GetComponent<BasicEnemyController>().AttackMaxValue;
            LifeCurrentValue -= damageReceived;
            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(), DamageType.DAMAGE, 1.0f, false);
        }
    }
}

using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : Character
{
    [SerializeField] private StatUI lifeBar;

    [NonSerialized] public StatField invincibilityTime;

    [SerializeField] private float initInvincibilityTime;

    private Animator movementAnimator;

    [SerializeField] private GameObject razakusMenu;

    private float timeSinceLastAttack = 0, timeSinceLastHit = 0;

    public GameObject projectile;

    private static PlayerScript _instance;

    public PlayerScript(StatField invincibilityTime)
    {
        this.invincibilityTime = invincibilityTime;
    }

    public static PlayerScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<PlayerScript>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        movementAnimator = GetComponent<Animator>();

        if (razakusMenu.activeSelf)
        {
            razakusMenu.SetActive(!razakusMenu.activeSelf);
        }

        base.Start();

        lifeBar.Initialized(life.MyMaxValue, life.MyMaxValue);

        InitStatField(ref invincibilityTime, initInvincibilityTime, false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        HandleLayers();

        base.Update();

        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            razakusMenu.SetActive(!razakusMenu.activeSelf);
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
        GameObject newProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        newProjectile.GetComponent<ProjectileScript>().SetDirection(worldPosition, transform.position.x, transform.position.y);
        newProjectile.GetComponent<ProjectileScript>().MyDamage = attack.MyMaxValue;
        newProjectile.GetComponent<ProjectileScript>().MyKnockback = knockback.MyMaxValue;
        newProjectile.GetComponent<ProjectileScript>().MyRange = 1/range.MyMaxValue;

        //player lose one health per shot
        life.MyCurrentValue -= 1;
    }

    private void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("Walk Layer");

            movementAnimator.SetFloat("X_speed", direction.x);
            movementAnimator.SetFloat("Y_speed", direction.y);
        }
        else
        {
            ActivateLayer("Idle Layer");
        }
    }

    private void ActivateLayer(string layerName)
    {
        for (int i = 0; i < movementAnimator.layerCount; i++)
        {
            movementAnimator.SetLayerWeight(i, 0);
        }

        movementAnimator.SetLayerWeight(movementAnimator.GetLayerIndex(layerName), 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && timeSinceLastHit > invincibilityTime.MyMaxValue)
        {
            timeSinceLastHit = 0;

            var damageReceived = collision.gameObject.GetComponent<BasicEnemyController>().attack.MyMaxValue;
            life.MyCurrentValue -= damageReceived;
            lifeBar.MyCurrentValue -= damageReceived;
            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(CultureInfo.InvariantCulture), DamageType.PLAYER, 1.0f, false);
        }
    }
}

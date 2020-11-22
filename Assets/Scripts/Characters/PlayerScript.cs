using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class PlayerScript : Character
{
    [SerializeField] public StatField invincibilityTime;

    [SerializeField] public float initInvincibilityTime;

    [SerializeField] private int levelIgeirus;

    public int MyIgeirusLevel
    {
        get => levelIgeirus;

        set => levelIgeirus = value;
    }
    
    [SerializeField] private int levelUrbius;

    public int MyUrbiusLevel
    {
        get => levelUrbius;
        
        set => levelUrbius = value;
    }
    
    private bool isInvincible;
    
    private Coroutine invinsibleCoroutine;
    
    [SerializeField] private StatUI lifeBar;

    [SerializeField] private bool isProjectilesDisabled;

    private bool isInMenu = false;

    public float PlayerMaxLife
    {
        get => life.MyMaxValue;

        set
        {
            life.MyMaxValue = value;
            lifeBar.MyMaxValue = value;

            PlayerCurrentLife = PlayerCurrentLife;
        }
    }
    
    public float PlayerCurrentLife
    {
        get => life.MyCurrentValue;

        set
        {
            life.MyCurrentValue = value;
            lifeBar.MyCurrentValue = value;
        }
    }

    private Animator movementAnimator;
    private Light2D light2D;

    public GameObject projectile;
    
    private bool isAttacking;
    private bool isLosingHealthWhenAttacking = true;

    private Coroutine attackCoroutine;

    private static PlayerScript _instance;

    [SerializeField] private float mouseTimeDirection;

    private bool isLookingMouse;

    private Coroutine mouseLookCoroutine;

    private static readonly int X = Animator.StringToHash("x");
    private static readonly int Y = Animator.StringToHash("y");
    
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

        base.Start();

        lifeBar.Initialized(life.MyMaxValue, life.MyMaxValue);

        light2D = GetComponent<Light2D>();

        InitStatField(ref invincibilityTime, initInvincibilityTime, false);

        // TODO : Temporaire
        if (!isProjectilesDisabled) TimerManager.MyInstance.MyTimer = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        HandleLayers();

        base.Update();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (!isInMenu)
        {
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
        }

        if (Input.GetMouseButton(0) && !isAttacking && !isProjectilesDisabled)
        {
            FireProjectile();
        }
    }

    private Vector2 GetPlayerToDirection()
    {
        var position = transform.position;
        
        if (Camera.main is null) return position;
        
        //get mouse position in world space
        Vector2 screenPosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        
        //substract character position to change the origin of the projectile direction
        return worldPosition - new Vector2(position.x, position.y);
    }

    private void FireProjectile()
    {
        var position = transform.position;
            
        var projectileDirection = GetPlayerToDirection();

        var randomNumber = Random.Range(0, 100);
        var damageToDeal = attack.MyMaxValue;
        var isCrit = false;
        
        if (randomNumber <= critChance.MyMaxValue)
        {
            damageToDeal *= critDamage.MyMaxValue;
            isCrit = true;
        }

        //create projectile
        var newProjectile = Instantiate(projectile, new Vector3(position.x, position.y, -2), Quaternion.identity);
        newProjectile.GetComponent<ProjectileScript>().SetDirection(projectileDirection, position.x, position.y);
        newProjectile.GetComponent<ProjectileScript>().MyBaseDamage = damageToDeal;
        newProjectile.GetComponent<ProjectileScript>().MyKnockback = knockback.MyMaxValue;
        newProjectile.GetComponent<ProjectileScript>().MyRange = 1 / range.MyMaxValue;
        newProjectile.GetComponent<ProjectileScript>().isCrit = isCrit;
        
        //player lose one health per shot
        if (isLosingHealthWhenAttacking)
            PlayerCurrentLife -= 1;

        if (mouseLookCoroutine != null)
        {
            StopMouseLook();
        }

        mouseLookCoroutine = StartCoroutine(StartMouseLook());

        attackCoroutine = StartCoroutine(StartAttack());
    }

    private void HandleLayers()
    {
        if (IsMoving || isLookingMouse)
        {
            ActivateLayer("Walk Layer");

            if (isLookingMouse)
            {
                var newDir = GetPlayerToDirection().normalized;
                
                movementAnimator.SetFloat(X, newDir.x);
                movementAnimator.SetFloat(Y, newDir.y);
            }
            else
            {
                movementAnimator.SetFloat(X, direction.x);
                movementAnimator.SetFloat(Y, direction.y);
            }
        }
        else
        {
            ActivateLayer("Idle Layer");
        }
    }

    private IEnumerator StartMouseLook()
    {
        isLookingMouse = true;
        
        yield return new WaitForSeconds(mouseTimeDirection);
        
        StopMouseLook();
    }

    private void StopMouseLook()
    {
        isLookingMouse = false;

        if (mouseLookCoroutine != null)
        {
            StopCoroutine(mouseLookCoroutine);
        }
    }
    
    private IEnumerator StartAttack()
    {
        isAttacking = true;
        
        yield return new WaitForSeconds(1 / attackSpeed.MyMaxValue);
        
        StopAttack();
    }

    private void StopAttack()
    {
        isAttacking = false;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
    
    private IEnumerator StartInvincibility()
    {
        isInvincible = true;
        
        yield return new WaitForSeconds(invincibilityTime.MyMaxValue);
        
        StopInvincibility();
    }

    private void StopInvincibility()
    {
        isInvincible = false;

        if (invinsibleCoroutine != null)
        {
            StopCoroutine(invinsibleCoroutine);
        }
    }

    public IEnumerator StartNotLosingHealthWhenAttacking(float secondsOfInvincibility)
    {
        isLosingHealthWhenAttacking = false;

        yield return new WaitForSeconds(secondsOfInvincibility);

        isLosingHealthWhenAttacking = true;

        yield break;
    }


    private void ActivateLayer(string layerName)
    {
        for (var i = 0; i < movementAnimator.layerCount; i++)
        {
            movementAnimator.SetLayerWeight(i, 0);
        }

        movementAnimator.SetLayerWeight(movementAnimator.GetLayerIndex(layerName), 1);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isInvincible) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            invinsibleCoroutine = StartCoroutine(StartInvincibility());

            var damageReceived = collision.gameObject.GetComponent<BasicEnemyController>().attack.MyMaxValue;
            PlayerCurrentLife -= damageReceived;

            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(CultureInfo.InvariantCulture), DamageType.Player, 1.0f, false);
        }

    }

    public void ReceiveDmgFromProjectile(float damageReceived)
    {
        invinsibleCoroutine = StartCoroutine(StartInvincibility());
        print("touched");
        PlayerCurrentLife -= damageReceived;

        CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(CultureInfo.InvariantCulture), DamageType.Player, 1.0f, false);
    }
    public void SetIsInMenu(bool b)
    {
        isInMenu = b;
    }

    public bool GetIsInMenu()
    {
        return isInMenu;
    }

    public void SetPlayerLighting(float radius)
    {
        light2D.pointLightOuterRadius = radius;
        light2D.pointLightInnerRadius = radius / 10;
    }

    /*public float[] getStatMaxValues()
    {
        float[] playerStatMaxValues = new float[8];
        playerStatMaxValues[0] = PlayerScript.MyInstance.attack.MyMaxValue;
        playerStatMaxValues[1] = PlayerScript.MyInstance.life.MyMaxValue;
        playerStatMaxValues[2] = PlayerScript.MyInstance.range.MyMaxValue;
        playerStatMaxValues[3] = PlayerScript.MyInstance.movementSpeed.MyMaxValue;
        playerStatMaxValues[4] = PlayerScript.MyInstance.attackSpeed.MyMaxValue;
        playerStatMaxValues[5] = PlayerScript.MyInstance.critChance.MyMaxValue;
        playerStatMaxValues[6] = PlayerScript.MyInstance.critDamage.MyMaxValue;
        playerStatMaxValues[7] = PlayerScript.MyInstance.knockback.MyMaxValue;

        return playerStatMaxValues;
    }*/

}

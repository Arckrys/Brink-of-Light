using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyController : Character
{
    public float stoppingDistance;
    public float detectionRadius;
    protected float knockbackIntensity;
    public float knockbackResistance;
    protected Transform player;
    protected Animator gfxAnim;
    protected bool playerDetected;
    protected int knockbackTimer;
    protected int wanderTimer;
    protected Vector2 randomDirection;

    private CanvasGroup canvasGroupLifeBar;
    private Coroutine lifeBarCoroutine;
    private Animator lifeBarAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gfxAnim = transform.GetComponent<Animator>();
        playerDetected = false;
        knockbackTimer = 0;
        wanderTimer = 0;
        randomDirection = new Vector2();

        canvasGroupLifeBar = transform.Find("LifeCanvas").GetComponent<CanvasGroup>();
        lifeBarAnimator = transform.Find("LifeCanvas").GetComponent<Animator>();

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        UpdatePolygonCollider();

        //UpdateLifeBar();

        base.Update();
    }

    protected override void FixedUpdate()
    {
        

        base.FixedUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Spell"))
        {
            knockbackIntensity = collision.GetComponent<ProjectileScript>().MyKnockback;
            float damageReceived = collision.GetComponent<ProjectileScript>().MyDamage;
            life.MyCurrentValue -= damageReceived;
            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(), DamageType.DAMAGE, 1.0f, false);

            ShowLifeBar();

            playerDetected = true;
            if(knockbackIntensity - knockbackResistance > 0)
            {
                gfxAnim.SetBool("Knockback", true);
                knockbackTimer = 0;
            }
            
            if(life.MyCurrentValue == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ShowLifeBar()
    {
        if (lifeBarCoroutine != null)
        {
            StopCoroutine(lifeBarCoroutine);
        }

        lifeBarCoroutine = StartCoroutine(FadeOutLifeBar());
    }

    public IEnumerator FadeOutLifeBar()
    {
        float startAlpha = 1.0f;

        canvasGroupLifeBar.alpha = startAlpha;

        yield return new WaitForSeconds(3.0f);

        float rate = 2.5f;

        float progress = 0.0f;

        while (progress < 1.0)
        {
            canvasGroupLifeBar.alpha = Mathf.Lerp(startAlpha, 0, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        StopCoroutine(lifeBarCoroutine);
    }

    protected void Wander()
    {
        if(wanderTimer >= 100)
        {
            randomDirection.x = (Random.Range(-1,2));
            randomDirection.y = (Random.Range(-1, 2));
            direction = randomDirection;
            FaceDirection(randomDirection);
            wanderTimer = 0;
            print(randomDirection);
        }
        else
        {
            wanderTimer += 1;
        }
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            playerDetected = true;
        }
    }

    private void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    /*private void UpdateLifeBar()
    {
        Image content = life.GetComponent<Image>();

        if (content.fillAmount != life.MyCurrentValue / life.MyMaxValue)
        {
            content.fillAmount = life.MyCurrentValue / life.MyMaxValue;
        }
    }*/

    protected void FaceDirection(Vector2 direction)
    {
        if(direction.x == 0 && direction.y == 0)
        {
            gfxAnim.SetBool("FacingRight", false);
            gfxAnim.SetBool("FacingLeft", false);
            gfxAnim.SetBool("FacingDown", false);
            gfxAnim.SetBool("FacingUp", false);
            gfxAnim.SetBool("Idle", true);
        }
        else
        {
            gfxAnim.SetBool("Idle", false);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 40 && angle > -40)
            {
                gfxAnim.SetBool("FacingRight", true);
                gfxAnim.SetBool("FacingLeft", false);
                gfxAnim.SetBool("FacingDown", false);
                gfxAnim.SetBool("FacingUp", false);
            }
            if (angle <= 135 && angle >= 45)
            {
                gfxAnim.SetBool("FacingRight", false);
                gfxAnim.SetBool("FacingLeft", false);
                gfxAnim.SetBool("FacingDown", false);
                gfxAnim.SetBool("FacingUp", true);
            }
            if (angle < -140 || angle > 140)
            {
                gfxAnim.SetBool("FacingRight", false);
                gfxAnim.SetBool("FacingLeft", true);
                gfxAnim.SetBool("FacingDown", false);
                gfxAnim.SetBool("FacingUp", false);
            }
            if (angle <= -45 && angle >= -135)
            {
                gfxAnim.SetBool("FacingRight", false);
                gfxAnim.SetBool("FacingLeft", false);
                gfxAnim.SetBool("FacingDown", true);
                gfxAnim.SetBool("FacingUp", false);
            }
        }
    }

}

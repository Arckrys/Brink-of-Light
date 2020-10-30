using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyController : Character
{
    public float stoppingDistance;
    public float detectionRadius;
    public float knockbackIntesity;
    public Transform player;
    private Animator gfxAnim;
    private bool playerDetected;
    private int knockbackTimer;
    private int wanderTimer;
    private Vector2 randomDirection;

    private CanvasGroup canvasGroupLifeBar;
    private Coroutine lifeBarCoroutine;
    private Animator lifeBarAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stoppingDistance = 0;
        gfxAnim = transform.GetComponent<Animator>();
        playerDetected = false;
        knockbackTimer = 0;
        knockbackIntesity = 1;
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

        UpdateLifeBar();

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")))
            {
                direction = player.position - transform.position;
                //print(direction);
                Vector2 facingDirection = player.position - transform.position;
                FaceDirection(facingDirection);
                
            }
        }
        else
        {
            Wander();
        }
        if (gfxAnim.GetBool("Knockback")){
            knockbackTimer += 1;
            direction = -1 * knockbackIntesity * (player.position - transform.position);
            if (knockbackTimer > 10)
            {
                gfxAnim.SetBool("Knockback", false);
            }
        }

        base.FixedUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Spell") && !(gfxAnim.GetBool("Knockback")))
        {
            life.MyCurrentValue -= 1;
            CombatTextManager.MyInstance.CreateText(transform.position, 1.0f.ToString(), DamageType.DAMAGE, 1.0f, false);

            ShowLifeBar();

            playerDetected = true;
            gfxAnim.SetBool("Knockback", true);
            knockbackTimer = 0;
            transform.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position));
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

    private void Wander()
    {
        print(wanderTimer);
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

    private void UpdateLifeBar()
    {
        Image content = life.GetComponent<Image>();

        if (content.fillAmount != life.MyCurrentValue / life.MyMaxValue)
        {
            content.fillAmount = life.MyCurrentValue / life.MyMaxValue;
        }
    }

    private void FaceDirection(Vector2 direction)
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

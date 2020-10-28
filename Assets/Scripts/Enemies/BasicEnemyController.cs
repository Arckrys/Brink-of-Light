using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyController : Character
{
    public float speed;
    public float stoppingDistance;
    public float detectionRadius;
    public Transform player;
    private Animator gfxAnim;
    private bool playerDetected;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stoppingDistance = 0;
        gfxAnim = transform.GetComponent<Animator>();
        playerDetected = false;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                direction = player.position - transform.position;
                //print(direction);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if ( angle < 40 && angle > -40)
                {
                    gfxAnim.SetBool("FacingRight", true);
                    gfxAnim.SetBool("FacingLeft", false);
                    gfxAnim.SetBool("FacingDown", false);
                    gfxAnim.SetBool("FacingUp", false);
                }
                if (angle < 130 && angle > 50)
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
                if (angle < -50 && angle > -130)
                {
                    gfxAnim.SetBool("FacingRight", false);
                    gfxAnim.SetBool("FacingLeft", false);
                    gfxAnim.SetBool("FacingDown", true);
                    gfxAnim.SetBool("FacingUp", false);
                }
                //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < detectionRadius)
            {
                playerDetected = true;
            }
        }

        UpdatePolygonCollider();

        UpdateLifeBar();

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Spell"))
        {
            life.MyCurrentValue -= 1;

            CombatTextManager.MyInstance.CreateText(transform.position, 1.0f.ToString(), DamageType.DAMAGE, 1.0f, false);
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

        if (life.MyCurrentValue == life.MyMaxValue)
        {
            life.transform.parent.gameObject.SetActive(false);
        }
        else if (content.fillAmount != life.MyCurrentValue / life.MyMaxValue)
        {
            life.transform.parent.gameObject.SetActive(true);

            content.fillAmount = Mathf.Lerp(content.fillAmount, life.MyCurrentValue / life.MyMaxValue, Time.deltaTime);
        }
    }
}

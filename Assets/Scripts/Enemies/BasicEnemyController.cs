using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float detectionRadius;
    private GameObject gfx;
    public Transform player;
    private Animator gfxAnim;
    private bool playerDetected;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gfx = transform.Find("EnemyGFX").gameObject;
        stoppingDistance = 0;
        gfxAnim = gfx.GetComponent<Animator>();
        playerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                Vector2 direction = player.position - transform.position;
                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if ( angle < 40 & angle > -40)
                {
                    gfxAnim.SetBool("FacingRight", true);
                    gfxAnim.SetBool("FacingLeft", false);
                    gfxAnim.SetBool("FacingDown", false);
                    gfxAnim.SetBool("FacingUp", false);
                }
                if (angle < 130 & angle > 50)
                {
                    gfxAnim.SetBool("FacingRight", false);
                    gfxAnim.SetBool("FacingLeft", false);
                    gfxAnim.SetBool("FacingDown", false);
                    gfxAnim.SetBool("FacingUp", true);
                }
                if (angle < -140 | angle > 140)
                {
                    gfxAnim.SetBool("FacingRight", false);
                    gfxAnim.SetBool("FacingLeft", true);
                    gfxAnim.SetBool("FacingDown", false);
                    gfxAnim.SetBool("FacingUp", false);
                }
                if (angle < -50 & angle > -130)
                {
                    gfxAnim.SetBool("FacingRight", false);
                    gfxAnim.SetBool("FacingLeft", false);
                    gfxAnim.SetBool("FacingDown", true);
                    gfxAnim.SetBool("FacingUp", false);
                }
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); ;

            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < detectionRadius)
            {
                playerDetected = true;
            }
        }
    }
}

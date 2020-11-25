using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurusScript : BasicEnemyController
{
    private bool dashing;
    public int dashDistance;
    public float dashSpeedMultiplicator;
    public int stunTime;
    private int stunTimer;
    private int dashCounter;
    public GameObject camera;
    Vector3 initialPosition;
    private float shakeDuration;
    private bool shaking;
    public GameObject playerGameObject;
    private bool touchedPlayer;

    protected override void Start()
    {
        dashCounter = 0;
        stunTimer = 0;
        base.Start();
        initialPosition = camera.transform.localPosition;
        movementSpeed.MyMaxValue = playerGameObject.GetComponent<PlayerScript>().initMovementSpeed + 0.1f;
    }

    protected override void FixedUpdate()
    {
        if (!CanvasTransitionScript.MyInstance.isDoingTransition)
        {
            Behaviour();
        }
        base.FixedUpdate();
    }


    //Verification des collision pour arrêter le sprint en cas de collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Spell") && !collision.gameObject.tag.Equals("Player"))
        {
            dashCounter = 0;
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            direction = -1 * direction;
            touchedPlayer = true;
        }

        if (collision.gameObject.layer == 12 && !touchedPlayer)
        {
            Stun();
        }
    }



    /// <summary>
    /// Fonction ayant pour but de stocker le comportement de l'ennemi
    /// </summary>
    private void Behaviour()
    {
        if (playerDetected)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Stun")) && !dashing)
            {
                MoveToPlayer();
            }
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if (!(gfxAnim.GetBool("Stun")) && !dashing)
                {

                    movementSpeed.MyMaxValue *= dashSpeedMultiplicator;
                    direction = player.position - transform.position;
                    Move();
                    FaceDirection(direction, gfxAnim);
                    dashCounter = dashDistance;
                    dashing = true;
                    touchedPlayer = false;
                    gfxAnim.SetBool("Dashing", true);
                    gameObject.layer = 13;
                }
            }
            if (dashing)
            {
                if (dashCounter == 0)
                {
                    direction = player.position - transform.position;
                    FaceDirection(direction, gfxAnim);
                    touchedPlayer = false;
                    dashCounter = dashDistance;
                    Move();
                }

                if (dashCounter > 0)
                {
                    Move();
                    dashCounter -= 1;
                }
            }
        }
        else
        {
            Wander();
        }

        if (gfxAnim.GetBool("Stun") && !dashing)
        {
            stunTimer += 1;
            Move();
            if (stunTimer > stunTime)
            {
                gfxAnim.SetBool("Stun", false);
                Move();
            }
        }
        if (shaking)
        {
            if (shakeDuration > 0)
            {
                camera.transform.localPosition = initialPosition + Random.insideUnitSphere * 2;

                shakeDuration -= 1f;
            }
            else
            {
                shakeDuration = 0f;
                shaking = false;
                camera.transform.localPosition = initialPosition;

            }
        }
    }

    /// <summary>
    /// Fonction permettant de gérer la phase de Stun du boss
    /// </summary>
    private void Stun()
    {
        
        gfxAnim.SetBool("Stun", true);
        stunTimer = 0;
        if (dashing)
        {
            movementSpeed.MyMaxValue /= dashSpeedMultiplicator;
            dashing = false;
            dashCounter = 0;
            shakeDuration = 8f;
            shaking = true;
        }
        gameObject.layer = 10;
        direction = (player.position - transform.position) * 0;
        Move();
    }
    
}

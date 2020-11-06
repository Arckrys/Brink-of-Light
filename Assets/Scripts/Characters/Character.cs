using UnityEngine;
using System.Collections;
using System.Globalization;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public StatUI life;

    [SerializeField] private float initLife;

    [SerializeField] public StatField attack;

    [SerializeField] private float initAttack;

    [SerializeField] public StatField movementSpeed;

    [SerializeField] private float initMovementSpeed;

    [SerializeField] public StatField critChance;

    [SerializeField] private float initCritChance;
    
    [SerializeField] public StatField critDamage;

    [SerializeField] private float initCritDamage;

    [SerializeField] public StatField range;

    [SerializeField] private float initRange;

    [SerializeField] public StatField attackSpeed;

    [SerializeField] private float initAttackSpeed;
    
    [SerializeField] public StatField knockback;

    [SerializeField] private float initKnockback;

    private Rigidbody2D myRigidbody;

    protected Vector2 direction;

    private Coroutine damageOnTimeCoroutine;
    private bool isTakingDamageOnTime;

    protected bool IsMoving => direction.x != 0 || direction.y != 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        life.Initialized(initLife, initLife);
        InitStatField(ref attack, initAttack, false);
        InitStatField(ref movementSpeed, initMovementSpeed, false);
        InitStatField(ref critChance, initCritChance, false);
        InitStatField(ref critDamage, initCritDamage, false);
        InitStatField(ref range, initRange, false);
        InitStatField(ref attackSpeed, initAttackSpeed, false);
        InitStatField(ref knockback, initKnockback, false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdatePolygonCollider();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void InitStatField(ref StatField stat, float initValue, bool variable)
    {
        if (!stat)
        {
            stat = new StatField(initValue, initValue, variable);
        }
        
        stat.Initialized(initValue, initValue);
    }

    private void Move()
    {
        myRigidbody.velocity = direction.normalized * movementSpeed.MyCurrentValue;
    }

    private void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void ResetAnimator(Animator animator)
    {
        animator.SetBool("FacingRight", false);
        animator.SetBool("FacingLeft", false);
        animator.SetBool("FacingDown", false);
        animator.SetBool("FacingUp", false);
        animator.SetBool("Idle", false);
    }

    protected void FaceDirection(Vector2 newDir, Animator animator)
    {
        ResetAnimator(animator);
        
        if (newDir.x == 0 && newDir.y == 0)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            var angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
            
            if (angle < 40 && angle > -40)
            {
                animator.SetBool("FacingRight", true);
            }
            if (angle <= 135 && angle >= 45)
            {
                animator.SetBool("FacingUp", true);
            }
            if (angle < -140 || angle > 140)
            {
                animator.SetBool("FacingLeft", true);
            }
            if (angle <= -45 && angle >= -135)
            {
                animator.SetBool("FacingDown", true);
            }
        }
    }

    public IEnumerator StartDamageOnTime(float frequency, int maxTick, float damage)
    {
        isTakingDamageOnTime = true;
        int tick = 0;

        while (isTakingDamageOnTime)
        {
            life.MyCurrentValue -= damage;

            if (gameObject.GetComponent<BasicEnemyController>() != null)
            {
                life.MyCurrentValue -= damage;
                gameObject.GetComponent<BasicEnemyController>().ShowLifeBar();
            }
            else if (gameObject.GetComponent<PlayerScript>() != null)
            {
                PlayerScript.MyInstance.PlayerCurrentLife -= damage;
            }

            CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(CultureInfo.InvariantCulture), DamageType.DamageOnTime, 1.0f, false);

            if ((gameObject.GetComponent<BasicEnemyController>() != null && life.MyCurrentValue == 0) || (gameObject.GetComponent<PlayerScript>() != null && PlayerScript.MyInstance.PlayerCurrentLife == 0))
            {
                Destroy(gameObject);
            }

            tick++;
            if (tick == maxTick)
                isTakingDamageOnTime = false;

            yield return new WaitForSeconds(frequency);
        }
    }
}

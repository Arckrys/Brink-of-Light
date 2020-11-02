using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat life;

    [SerializeField] private float initLife;

    [SerializeField] protected Stat attack;

    [SerializeField] private float initAttack;

    [SerializeField] protected Stat mouvementSpeed;

    [SerializeField] private float initMouvementSpeed;

    [SerializeField] protected Stat critChance;

    [SerializeField] private float initCritChance;

    [SerializeField] protected Stat range;

    [SerializeField] private float initRange;

    [SerializeField] protected Stat attackSpeed;

    [SerializeField] private float initAttackSpeed;
    
    [SerializeField] protected Stat knockback;

    [SerializeField] private float initKnockback;

    private Rigidbody2D rigidbody;

    protected Vector2 direction;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    public float LifeMaxValue
    {
        get
        {
            return life.MyMaxValue;
        }

        set
        {
            life.MyMaxValue = value;
        }
    }

    public float LifeCurrentValue
    {
        get
        {
            return life.MyCurrentValue;
        }

        set
        {
            life.MyCurrentValue = value;
        }
    }

    public float AttackMaxValue
    {
        get
        {
            return attack.MyMaxValue;
        }

        set
        {
            attack.MyMaxValue = value;
            attack.MyCurrentValue = value;
        }
    }

    public float MovementSpeedMaxValue
    {
        get
        {
            return mouvementSpeed.MyMaxValue;
        }

        set
        {
            mouvementSpeed.MyMaxValue = value;
            mouvementSpeed.MyCurrentValue = value;
        }
    }

    public float CritChanceMaxValue
    {
        get
        {
            return critChance.MyMaxValue;
        }

        set
        {
            critChance.MyMaxValue = value;
            critChance.MyCurrentValue = value;
        }
    }

    public float RangeMaxValue
    {
        get
        {
            return range.MyMaxValue;
        }

        set
        {
            range.MyMaxValue = value;
            range.MyCurrentValue = value;
        }
    }

    public float AttackSpeedMaxValue
    {
        get
        {
            return attackSpeed.MyMaxValue;
        }

        set
        {
            attackSpeed.MyMaxValue = value;
            attackSpeed.MyCurrentValue = value;
        }
    }

    public float KnockbackMaxValue
    {
        get
        {
            return knockback.MyMaxValue;
        }

        set
        {
            knockback.MyMaxValue = value;
            knockback.MyCurrentValue = value;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        life.Initialized(initLife, initLife);
        attack.Initialized(initAttack, initAttack);
        mouvementSpeed.Initialized(initMouvementSpeed, initMouvementSpeed);
        critChance.Initialized(initCritChance, initCritChance);
        range.Initialized(initRange, initRange);
        attackSpeed.Initialized(initAttackSpeed, initAttackSpeed);
        knockback.Initialized(initKnockback, initKnockback);
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

    private void Move()
    {
        rigidbody.velocity = direction.normalized * mouvementSpeed.MyCurrentValue;
    }

    private void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    protected void FaceDirection(Vector2 direction, Animator Anim)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            Anim.SetBool("FacingRight", false);
            Anim.SetBool("FacingLeft", false);
            Anim.SetBool("FacingDown", false);
            Anim.SetBool("FacingUp", false);
            Anim.SetBool("Idle", true);
        }
        else
        {
            Anim.SetBool("Idle", false);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 40 && angle > -40)
            {
                Anim.SetBool("FacingRight", true);
                Anim.SetBool("FacingLeft", false);
                Anim.SetBool("FacingDown", false);
                Anim.SetBool("FacingUp", false);
            }
            if (angle <= 135 && angle >= 45)
            {
                Anim.SetBool("FacingRight", false);
                Anim.SetBool("FacingLeft", false);
                Anim.SetBool("FacingDown", false);
                Anim.SetBool("FacingUp", true);
            }
            if (angle < -140 || angle > 140)
            {
                Anim.SetBool("FacingRight", false);
                Anim.SetBool("FacingLeft", true);
                Anim.SetBool("FacingDown", false);
                Anim.SetBool("FacingUp", false);
            }
            if (angle <= -45 && angle >= -135)
            {
                Anim.SetBool("FacingRight", false);
                Anim.SetBool("FacingLeft", false);
                Anim.SetBool("FacingDown", true);
                Anim.SetBool("FacingUp", false);
            }
        }
    }
}

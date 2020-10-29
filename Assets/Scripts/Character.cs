using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody.velocity = direction.normalized * mouvementSpeed.MyCurrentValue;
    }
}

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

    private Rigidbody2D rigidbody;

    protected Vector2 direction;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody.velocity = direction.normalized * mouvementSpeed.MyCurrentValue;
    }
}

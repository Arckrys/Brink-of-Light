using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat life;

    [SerializeField] protected float initLife;

    [SerializeField] private float mouvementSpeed;

    

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
        //life.Initialized(initLife, initLife);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rigidbody.velocity = direction.normalized * mouvementSpeed;
    }

    
}

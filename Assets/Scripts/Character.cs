using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat life;

    [SerializeField] private float initLife;

    [SerializeField] private float speed;

    protected Vector2 direction;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        life.Initialized(initLife, initLife);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

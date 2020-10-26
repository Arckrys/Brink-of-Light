using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat life;

    [SerializeField] private float initLife;

    [SerializeField] private float mouvementSpeed;

    private Animator mouvementAnimator;

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

        mouvementAnimator = GetComponent<Animator>();

        life.Initialized(initLife, initLife);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rigidbody.velocity = direction.normalized * mouvementSpeed;
    }

    private void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("Walk Layer");

            mouvementAnimator.SetFloat("X_speed", direction.x);
            mouvementAnimator.SetFloat("Y_speed", direction.y);
        }
        else
        {
            ActivateLayer("Idle Layer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < mouvementAnimator.layerCount; i++)
        {
            mouvementAnimator.SetLayerWeight(i, 0);
        }

        mouvementAnimator.SetLayerWeight(mouvementAnimator.GetLayerIndex(layerName), 1);
    }
}

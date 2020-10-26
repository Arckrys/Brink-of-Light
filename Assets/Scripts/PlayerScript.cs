using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character
{
    private float x = 0;
    private float y = 0;
    private float previous_X = 0;
    private float previous_Y = 0;


    private float timeSinceLastAttack = 0;
    public float nextAttackDelay = 0.1f;
    public float movingSpeed = 5f;

    private Animator anim;
    public GameObject Projectile;

    // Start is called before the first frame update
    protected override void Start()
    {
       // transform.SetParent(GameObject.Find("Canvas").transform, false);
        anim = GetComponent<Animator>();

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //timeSinceLastAttack += Time.deltaTime;

        GetInput();
        

        /*if (Input.GetMouseButton(0) && timeSinceLastAttack > nextAttackDelay)
        {
            FireProjectile();
            timeSinceLastAttack = 0;
        }*/

        base.Update();
    }

    public void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }
    }

    public void FireProjectile()
    {
        //get mouse position in world space
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //substract character position to change the origin of the projectile direction
        worldPosition = worldPosition - new Vector2(transform.position.x, transform.position.y);

        //create projectile
        GameObject projectile = Instantiate(Projectile, new Vector3(x, y, 0), Quaternion.identity);
        projectile.GetComponent<ProjectileScript>().SetDirection(worldPosition, x, y);
    }
}

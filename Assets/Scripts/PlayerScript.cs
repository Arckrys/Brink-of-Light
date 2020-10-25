using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
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
    void Start()
    {
       // transform.SetParent(GameObject.Find("Canvas").transform, false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
            x -= movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            x += movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
            y += movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            y -= movingSpeed * Time.deltaTime;

        {
            float x_move = (x - previous_X) * 100;
            float y_move = (y - previous_Y) * 100;

            anim.SetFloat("X_speed", x_move);
            anim.SetFloat("Y_speed", y_move);
        }
        

        if (Input.GetMouseButton(0) && timeSinceLastAttack > nextAttackDelay)
        {
            FireProjectile();
            timeSinceLastAttack = 0;
        }

        transform.localPosition = new Vector3(x, y, 0);
        previous_X = x;
        previous_Y = y;
        
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

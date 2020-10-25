using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    private float x = 0;
    private float y = 0;

    private float time = 0;
    public float attackSpeed = 0.1f;
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
        time += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            x -= movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            x += movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow))
            y += movingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow))
            y -= movingSpeed * Time.deltaTime;

        {
            float x_speed = Input.GetAxis("Horizontal");
            float y_speed = Input.GetAxis("Vertical");
            anim.SetFloat("X_speed", x_speed);
            anim.SetFloat("Y_speed", y_speed);
        }
        

        if (Input.GetMouseButton(0) && time > attackSpeed)
        {
            FireProjectile();
            time = 0;
        }

        transform.localPosition = new Vector3(x, y, 0);
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

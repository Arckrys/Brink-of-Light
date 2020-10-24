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
    private float attackSpeed = 0.1f;

    public GameObject Projectile;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            x -= 0.5f;

        if (Input.GetKey(KeyCode.RightArrow))
            x += 0.5f;

        if (Input.GetKey(KeyCode.UpArrow))
            y += 0.5f;

        if (Input.GetKey(KeyCode.DownArrow))
            y -= 0.5f;

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

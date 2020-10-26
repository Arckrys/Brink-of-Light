using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    private float x, y, spriteScaleX, spriteScaleY, timeTemp = 0;
    private float xDirection, yDirection;

    public float projectileShrinkSpeed = 0.05f;
    public float projectileSpeed = 10f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        boxCollider = GetComponent<BoxCollider2D>() as BoxCollider2D;

        Vector3 newSize = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0);
        boxCollider.size = newSize;
    }


    // Update is called once per frame
    void Update()
    {
        //update position variables
        x += xDirection * Time.deltaTime * projectileSpeed;
        y += yDirection * Time.deltaTime * projectileSpeed;

        //update physical position
        transform.localPosition = new Vector3(x, y, 0);

        timeTemp += Time.deltaTime;

        if (timeTemp > 0.05f)
        {
            spriteScaleX = transform.localScale.x;
            spriteScaleY = transform.localScale.y;

            /*myHeight = GetComponent<RectTransform>().rect.height - 1;
            myWidth = GetComponent<RectTransform>().rect.width - 1;*/

            if (spriteScaleX > 0 && spriteScaleY > 0)
            {
                Vector3 newScale = new Vector3(spriteScaleX - projectileShrinkSpeed, spriteScaleY - projectileShrinkSpeed, 0);
                transform.localScale = newScale;

                Vector3 newSize = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0);
                boxCollider.size = newSize;
                /*GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().rect.height - 1);
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width - 1);*/
                timeTemp = 0;
            }

            else
                Destroy(gameObject);
        }


        //destroy projectile if out of screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction, float initialX, float initialY)
    {
        x = initialX;
        y = initialY;

        direction = direction.normalized;

        xDirection = direction.x;
        yDirection = direction.y;
    }
}

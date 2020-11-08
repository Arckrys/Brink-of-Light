using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected string itemName;
    protected Sprite itemSprite;

    public virtual void SetName(string itemName)
    {
        this.itemName = itemName;
    }

    public string GetName()
    {
        return itemName;
    }

    protected void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }
}

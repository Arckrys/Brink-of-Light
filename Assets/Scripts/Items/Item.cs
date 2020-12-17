using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected string itemName;
    protected Sprite itemSprite;
    protected bool isItemSold = false;
    protected int myGoldCost;

    public virtual void SetName(string itemName)
    {
        this.itemName = itemName;
    }

    public string GetName()
    {
        return itemName;
    }

    public void SetIsItemSold(bool b, int cost)
    {
        isItemSold = b;
        myGoldCost = cost;
    }

    protected void UpdatePolygonCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }
}

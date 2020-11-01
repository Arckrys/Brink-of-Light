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
}

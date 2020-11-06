using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipmentScript : Item
{
    public override void SetName(string itemName)
    {
        base.SetName(itemName);
        itemSprite = Resources.Load<Sprite>("Images/Items/Equipment/" + itemName);
        GetComponent<SpriteRenderer>().sprite = itemSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);
            GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>().ApplyItemModifications(itemName);
        }
    }
}

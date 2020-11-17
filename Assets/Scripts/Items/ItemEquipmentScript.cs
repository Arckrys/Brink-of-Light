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
        UpdatePolygonCollider();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isItemSold)
        {
            //débite l'argent
            if (PlayerScript.MyInstance.GetComponent<CurrenciesScript>().purchaseForGold(myGoldCost))
                canBuyItem = true;
        }

        if (other.gameObject.name == "Player" && ((canBuyItem && isItemSold) || !isItemSold))
        {
            Destroy(gameObject);
            GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>().ApplyItemModifications(itemName);

            if(isItemSold)
            {
                transform.parent.gameObject.GetComponent<SpawnItemScript>().DestroyPriceText();
            }
        }
    }
}

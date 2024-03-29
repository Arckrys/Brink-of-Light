﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //when colliding with the player
        if (other.gameObject.name == "Player")
        {
            //if the item is to be bought and the player has enough gold to do so, of if the item is not sold
            if((PlayerScript.MyInstance.GetComponent<CurrenciesScript>().PurchaseForGold(myGoldCost) && isItemSold) || !isItemSold) 
            {
                //destroy the game object
                Destroy(gameObject);
                GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>().ApplyItemModifications(itemName);

                //Start Item Name Popup coroutine
                ItemsManagerScript.MyInstance.StartCoroutine("FadingItemPopup", itemName);

                //destroy the price text
                if (isItemSold)
                {
                    transform.parent.gameObject.GetComponent<SpawnItemScript>().DestroyPriceText();
                }
            }
        }
    }
}

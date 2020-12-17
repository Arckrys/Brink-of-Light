using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemConsumableScript : Item
{
    private float timeSincePickup = 0;

    public override void SetName(string itemName)
    {
        base.SetName(itemName);
        itemSprite = Resources.Load<Sprite>("Images/Items/Consommable/" + itemName);
        GetComponent<SpriteRenderer>().sprite = itemSprite;
        UpdatePolygonCollider();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string colliderName = other.gameObject.name;

        //when colliding with the player
        if (colliderName == "Player" && timeSincePickup > 1f)
        {
            //if the item is to be bought and the player has enough gold to do so, of if the item is not sold
            if ((isItemSold && PlayerScript.MyInstance.GetComponent<CurrenciesScript>().PurchaseForGold(myGoldCost)) || !isItemSold)
            {
                //destroy the price text
                if (isItemSold)
                {
                    transform.parent.gameObject.GetComponent<SpawnItemScript>().DestroyPriceText();
                }

                isItemSold = false;

                //display the consumable item on the UI
                Image image = GameObject.Find("ConsumableItemUI").GetComponent<Image>();
                image.sprite = itemSprite;
                Color tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;

                timeSincePickup = 0;

                ItemsManagerScript itemsManager = GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>();

                //if the player has no consumable item held
                if (itemsManager.PlayerConsumableItem == null)
                {
                    //get the new item and destroy the prefab of the object
                    itemsManager.PlayerConsumableItem = itemName;
                    SetName(itemsManager.PlayerConsumableItem);
                    Destroy(gameObject);
                }

                else
                {
                    string tempName;

                    //swap player's consumable item with the one on the ground
                    tempName = itemName;
                    SetName(itemsManager.PlayerConsumableItem);
                    itemsManager.PlayerConsumableItem = tempName;
                }
            }
        }        
    }

    private void FixedUpdate()
    {
        timeSincePickup += Time.deltaTime;
    }
}

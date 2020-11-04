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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string colliderName = other.gameObject.name;

        if (colliderName == "Player" && timeSincePickup > 1.5f)
        {
            timeSincePickup = 0;

            ItemsManagerScript itemsManager = GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>();

            if (itemsManager.PlayerConsumableItem == null)
            {
                itemsManager.PlayerConsumableItem = itemName;
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

            Image image = GameObject.Find("ConsumableItemUI").GetComponent<Image>();
            image.sprite = itemSprite;
            Color tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
        }
    }

    private void FixedUpdate()
    {
        timeSincePickup += Time.deltaTime;
    }
}

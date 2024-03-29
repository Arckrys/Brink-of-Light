﻿using UnityEngine;

public class LootManager : MonoBehaviour
{
    private string itemName;
    
    private int itemType;

    private int soulAmount;

    private int goldAmount;

    // Pick up currencies and/or item in loot bag
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var currency = CurrenciesScript.MyInstance;
            var itemManager = ItemsManagerScript.MyInstance;

            currency.AddSouls(soulAmount);
            currency.AddGold(goldAmount);

            if (itemName != null)
            {
                if (itemType > 0) itemManager.ApplyItemModifications(itemName);

                else
                {
                    GameObject consumableItem = ItemsManagerScript.MyInstance.CreateConsumableItem(transform.position, itemName);
                    consumableItem.transform.SetParent(this.transform.parent);
                }
            }

            Destroy(gameObject);
        }
    }

    public void CreateBag(string item, int type, int soul, int gold)
    {
        itemName = item;
        itemType = type;
        soulAmount = soul;
        goldAmount = gold;

        //print(soulAmount);
        //print(goldAmount);
    }
}

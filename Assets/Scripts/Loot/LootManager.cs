using UnityEngine;

public class LootManager : MonoBehaviour
{
    private string itemName;
    
    private int itemType;

    private int soulAmount;

    private int goldAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var currency = CurrenciesScript.MyInstance;
            var itemManager = ItemsManagerScript.MyInstance;

            currency.addSouls(soulAmount);
            currency.addGold(goldAmount);

            if (itemName != null)
            {
                if (itemType > 0) itemManager.ApplyItemModifications(itemName);

                else
                    ItemsManagerScript.MyInstance.CreateConsumableItem(transform.position, itemName);
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

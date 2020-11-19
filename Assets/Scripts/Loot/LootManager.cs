using UnityEngine;

public class LootManager : MonoBehaviour
{
    private string itemName;
    
    private int itemType;

    private int soulAmount;

    private int goldAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var currency = CurrenciesScript.MyInstance;
        var itemManager = ItemsManagerScript.MyInstance;
        
        currency.addSouls(soulAmount);
        currency.addGold(goldAmount);
        
        if (itemType > 0) itemManager.ApplyItemModifications(itemName);
        
        Destroy(gameObject);
    }

    public void CreateBag(string item, int type, int soul, int gold)
    {
        itemName = item;
        itemType = type;
        soulAmount = soul;
        goldAmount = gold;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnItemScript : MonoBehaviour
{
    [SerializeField] private bool isEquipment;
    [SerializeField] private bool isItemSold;
    [SerializeField] private string specificItemName;
    [SerializeField] private GameObject itemPriceGameObject;

    private int itemPrice;
    private GameObject itemSpawned;
    private Text priceText;

    // Start is called before the first frame update
    void Start()
    {
        GameObject itemManagerGameObject = GameObject.Find("ItemManager");
        ItemsManagerScript itemManagerScript = itemManagerGameObject.GetComponent<ItemsManagerScript>();
        List<string> myItemList;


        if (isEquipment)
        {
            myItemList = itemManagerScript.GetItemsEquipmentList();

            if (specificItemName != "")
                itemSpawned = itemManagerScript.CreateEquipmentItem(transform.position, specificItemName);

            else
                itemSpawned = itemManagerScript.CreateEquipmentItem(transform.position, itemManagerScript.SelectRandomItem(myItemList));
        }           

        else
        {
            myItemList = itemManagerScript.GetItemsConsumableList();

            if (specificItemName != "")
                itemSpawned = itemManagerScript.CreateConsumableItem(transform.position, specificItemName);

            else
                itemSpawned = itemManagerScript.CreateConsumableItem(transform.position, itemManagerScript.SelectRandomItem(myItemList));
        }

        itemSpawned.transform.SetParent(this.transform);


        if (isItemSold)
        {
            priceText = Instantiate(itemPriceGameObject, transform).GetComponent<Text>();
            priceText.transform.SetParent(gameObject.transform.GetChild(0).transform);
            priceText.transform.position = new Vector2(transform.position.x, transform.position.y - 0.6f);

            //change the item's price
            if (isEquipment)
            {
                itemPrice = 25;
            }

            else
            {
                if (itemSpawned.GetComponent<Item>().GetName() == "Silex")
                    itemPrice = 15;

                else
                    itemPrice = 10;
            }

            itemSpawned.GetComponent<Item>().SetIsItemSold(true, itemPrice);
            priceText.text = itemPrice.ToString();
        }
    }

    public void DestroyPriceText()
    {
        Destroy(priceText);
    }
}

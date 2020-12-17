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

        //get the correct items list
        if (isEquipment)
        {
            myItemList = itemManagerScript.GetItemsEquipmentList();
        }          
        else
        {
            myItemList = itemManagerScript.GetItemsConsumableList();            
        }

        //if a name is specified, spawn the requested item
        if (specificItemName != "")
            itemSpawned = itemManagerScript.CreateConsumableItem(transform.position, specificItemName);
        //if no name is specified, spawn a random item
        else
            itemSpawned = itemManagerScript.CreateConsumableItem(transform.position, itemManagerScript.SelectRandomItem(myItemList));

        itemSpawned.transform.SetParent(this.transform);


        //if we want the item to be sold in a shop
        if (isItemSold)
        {
            //set the text showing the price
            priceText = Instantiate(itemPriceGameObject, transform).GetComponent<Text>();
            priceText.transform.SetParent(gameObject.transform.GetChild(0).transform);
            priceText.transform.position = new Vector2(transform.position.x, transform.position.y - 0.6f);

            //change the item's price
            if (isEquipment)
            {
                itemPrice = 100;
            }

            else
            {
                if (itemSpawned.GetComponent<Item>().GetName() == "Silex")
                    itemPrice = 35;

                else
                    itemPrice = 50;
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

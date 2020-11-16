using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemScript : MonoBehaviour
{
    [SerializeField] private bool isEquipment;
    [SerializeField] private string specificItemName;

    // Start is called before the first frame update
    void Start()
    {
        GameObject itemManagerGameObject = GameObject.Find("ItemManager");
        ItemsManagerScript itemManagerScript = itemManagerGameObject.GetComponent<ItemsManagerScript>();
        List<string> myItemList;


        if (isEquipment == true)
        {
            myItemList = itemManagerScript.GetItemsEquipmentList();

            if (specificItemName != "")
                itemManagerScript.CreateEquipmentItem(transform.position, specificItemName);

            else
                itemManagerScript.CreateEquipmentItem(transform.position, itemManagerScript.SelectRandomItem(myItemList));
        }           

        else
        {
            myItemList = itemManagerScript.GetItemsConsumableList();

            if (specificItemName != "")
                itemManagerScript.CreateConsumableItem(transform.position, specificItemName);

            else
                itemManagerScript.CreateConsumableItem(transform.position, itemManagerScript.SelectRandomItem(myItemList));
        }
    }
}

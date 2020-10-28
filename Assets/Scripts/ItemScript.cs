using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    private string itemName;

    public void SetName(string itemName)
    {
        this.itemName = itemName;
    }

    public string SetName()
    {
        return itemName;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);                        
        }
    }

    private void OnDestroy()
    {
        GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>().ApplyItemModifications(itemName);
    }
}

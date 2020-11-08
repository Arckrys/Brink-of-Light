using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    
    public static InventoryManager MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<InventoryManager>();
            }

            return _instance;
        }
    }
    
    [SerializeField] private List<SlotManager> items = new List<SlotManager>();

    private bool IsInventoryFull()
    {
        var isFull = true;

        foreach (var item in items.Where(item => item.MySlotValidity))
        {
            isFull = !item.MySlotValidity;
        }
        
        return isFull;
    }

    public void AddItem(string itemName)
    {
        if (IsInventoryFull()) return;
        
        var sprite = Resources.Load<Sprite>("Images/Items/Equipment/" + itemName);

        var index = -1;
        
        foreach (var item in items.Where(item => item.MySlotValidity && index == -1))
        {
            index = items.IndexOf(item);
        }

        items[index].UpdateSlot(sprite, itemName, !items[index].MySlotValidity);
    }
    
    private void RemoveItem()
    {
        
    }
}

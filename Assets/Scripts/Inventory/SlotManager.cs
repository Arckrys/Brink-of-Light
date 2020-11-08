using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private Image slotImage;

    [SerializeField] private bool isAvailable;

    private string itemName;

    public string MyItemName => itemName;

    public bool MySlotValidity => isAvailable;

    public void UpdateSlot(Sprite sprite, string name, bool available)
    {
        var image = slotImage.GetComponent<Image>();
        
        image.sprite = sprite;

        var tempColor = image.color;
        tempColor.a = available ? 0f : 1f;
        image.color = tempColor;

        itemName = name;

        isAvailable = available;
    }
}

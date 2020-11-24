using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipmentScript : Item
{

    [SerializeField] private GameObject popupPanel;
    private Image background;
    private Text itemText;

    private void Start()
    {
        GameObject popupPanel = ItemsManagerScript.MyInstance.popupPanel;
        background = popupPanel.GetComponentInChildren<Image>();
        itemText = popupPanel.GetComponentInChildren<Text>();
    }

    public override void SetName(string itemName)
    {
        base.SetName(itemName);
        itemSprite = Resources.Load<Sprite>("Images/Items/Equipment/" + itemName);
        GetComponent<SpriteRenderer>().sprite = itemSprite;
        UpdatePolygonCollider();
    }

    private IEnumerator FadingItemPopup()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            Color bgColor = background.color;
            Color textColor = itemText.color;

            //Debug.Log("help" + bgColor.a);

            bgColor.a = ft;
            textColor.a = ft;

            //background.color = bgColor;
            background.canvasRenderer.SetColor(bgColor);
            itemText.color = textColor;

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (isItemSold)
            {
                if (PlayerScript.MyInstance.GetComponent<CurrenciesScript>().PurchaseForGold(myGoldCost))
                    canBuyItem = true;
            }

            if((canBuyItem && isItemSold) || !isItemSold) 
            {
                Destroy(gameObject);
                GameObject.Find("ItemManager").GetComponent<ItemsManagerScript>().ApplyItemModifications(itemName);

                // TODO Start Item Name Popup coroutine
                StartCoroutine("FadingItemPopup");

                if (isItemSold)
                {
                    transform.parent.gameObject.GetComponent<SpawnItemScript>().DestroyPriceText();
                }
            }
        }
    }
}

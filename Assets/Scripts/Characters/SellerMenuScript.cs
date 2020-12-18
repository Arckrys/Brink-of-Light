using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for Urbius and Igeirus menus
public class SellerMenuScript : MonoBehaviour
{
    [SerializeField] private Text title;
    
    [SerializeField] private Text price;
    
    [SerializeField] private Image item;
    
    [SerializeField] private Text description;

    [SerializeField] private Button btnUpdate;
    
    [SerializeField] private Text congrats;

    [SerializeField] private int initEquipmentItems;
    
    [SerializeField] private int initConsumableItems;

    public int InitEquipmentItems => initEquipmentItems;
    
    public int InitConsumableItems => initConsumableItems;

    private NPCName sellerName;

    public NPCName MySellerName
    {
        set => sellerName = value;
    }

    private static SellerMenuScript _instance;
    
    public static SellerMenuScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<SellerMenuScript>();
            }

            return _instance;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        btnUpdate.onClick.AddListener(OnUpdatePressed);
        
        gameObject.SetActive(false);
    }

    // Gets max level according to items available in the collection
    private bool IsMaxLevel(int currentLevel, ICollection items)
    {
        return currentLevel == items.Count;
    }

    private void HideAll()
    {
        title.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
        btnUpdate.gameObject.SetActive(false);
        item.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
    }
    
    private void ActiveAll()
    {
        title.gameObject.SetActive(true);
        price.gameObject.SetActive(true);
        btnUpdate.gameObject.SetActive(true);
        item.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
    }

    // Update and display the right menu, Igeirus or Urbius
    public void UpdateUI()
    {
        var playerLevel = 0;
        
        switch (sellerName)
        {
            case NPCName.Igeirus:
                playerLevel = PlayerScript.MyInstance.MyIgeirusLevel;
                initEquipmentItems = ItemsManagerScript.MyInstance.NumberOfBaseEquipmentItems;
                if (IsMaxLevel(playerLevel + initEquipmentItems, ItemsManagerScript.MyInstance.EquipmentItems))
                {
                    HideAll();
                    congrats.gameObject.SetActive(true);
                }
                else
                {
                    congrats.gameObject.SetActive(false);
                    ActiveAll();
                    title.text = "Igeirus le Forgeron";
                    description.text = ItemsManagerScript.MyInstance.EquipmentItems[playerLevel + initEquipmentItems];
                    item.sprite = Resources.Load<Sprite>("Images/Items/Equipment/" + ItemsManagerScript.MyInstance.EquipmentItems[playerLevel + initEquipmentItems]);
                }
                break;
            case NPCName.Urbius:
                playerLevel = PlayerScript.MyInstance.MyUrbiusLevel;
                initConsumableItems = ItemsManagerScript.MyInstance.NumberOfBaseConsumableItems;
                if (IsMaxLevel(playerLevel + initConsumableItems, ItemsManagerScript.MyInstance.ConsumableItems))
                {
                    HideAll();
                    congrats.gameObject.SetActive(true);
                }
                else
                {
                    congrats.gameObject.SetActive(false);
                    ActiveAll();
                    title.text = "Urbius l'Alchimiste";
                    description.text = ItemsManagerScript.MyInstance.ConsumableItems[playerLevel + initConsumableItems];
                    item.sprite = Resources.Load<Sprite>("Images/Items/Consommable/" + ItemsManagerScript.MyInstance.ConsumableItems[playerLevel + initConsumableItems]);
                }
                break;
            case NPCName.Razakus:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        price.text = playerLevel * 50 + 50 + " Ames";
    }
    
    private static bool PurchaseSouls(int value)
    {
        return CurrenciesScript.MyInstance.PurchaseForSouls(value);
    }

    // Apply purchases made in Igeirus and Urbius' menus
    private void OnUpdatePressed()
    {
        switch (sellerName)
        {
            case NPCName.Igeirus:
                if (PurchaseSouls(PlayerScript.MyInstance.MyIgeirusLevel * 50 + 50))
                {
                    PlayerScript.MyInstance.MyIgeirusLevel += 1;
                    UpdateUI();
                    GameManager.MyInstance.SaveGame();
                }
                break;
            case NPCName.Urbius:
                if (PurchaseSouls(PlayerScript.MyInstance.MyUrbiusLevel * 50 + 50))
                {
                    PlayerScript.MyInstance.MyUrbiusLevel += 1;
                    UpdateUI();
                    GameManager.MyInstance.SaveGame();
                }
                break;
            case NPCName.Razakus:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

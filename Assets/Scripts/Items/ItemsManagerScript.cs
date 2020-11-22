﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManagerScript : MonoBehaviour
{
    public GameObject itemEquipmentGameObject;
    public GameObject itemConsumableGameObject;
    public GameObject player;

    private PlayerScript playerScript;
    private AudioSource audio;

    [SerializeField] private AudioClip equipmentPickupClip, consumableTriggerClip;

    [SerializeField] private GameObject combustibleGameObject;

    private static ItemsManagerScript instance;

    //à mettre sur playerscript
    private List<string> possessedItems = new List<string>();
    private string consumableItem = null;

    //available items
    private List<string> itemsEquipmentList = new List<string> {        
        "Allumettes",
        "Amulette du dragon",
        "Anneau du dragon",
        "Anneau du forgeron",
        "Bottes d'Hotavius",
        "Cape de vampire",
        "Lampe à huile d'Hotavius",
        "Sauce piquante"
    };

    private List<string> itemsConsumableList = new List<string> {
        "Potion de vitesse",
        "Potion de force",
        "Potion de lumière",
        "Parchemin de feu",
        "Parchemin de froid"
    };

    public List<string> EquipmentItems => itemsEquipmentList;
    
    public List<string> ConsumableItems => itemsConsumableList;
    
    private static ItemsManagerScript _instance;

    public static ItemsManagerScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ItemsManagerScript>();
            }

            return _instance;
        }
    }

    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();

        audio = GetComponent<AudioSource>();

        //ItemsTest();        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UseConsumableItem();
    }

    private void ItemsTest()
    {
        CreateEquipmentItem(new Vector3(4, 3, 0), "Lampe à huile d'Hotavius");
        CreateEquipmentItem(new Vector3(-2, 3, 0), SelectRandomItem(itemsEquipmentList));
        CreateEquipmentItem(new Vector3(4, -3, 0), SelectRandomItem(itemsEquipmentList));
        CreateEquipmentItem(new Vector3(-2, -3, 0), SelectRandomItem(itemsEquipmentList));
        CreateConsumableItem(new Vector3(1, 3, 0), "Parchemin de feu");
        CreateConsumableItem(new Vector3(1, -3, 0), "Parchemin de froid");
    }

    public string SelectRandomItem(List<string> itemList)
    {
        string randomItem = itemList[Random.Range(0, itemList.Count)];

        return randomItem;
    }

    public List<string> GetItemsEquipmentList()
    {
        return itemsEquipmentList;
    }

    public List<string> GetItemsConsumableList()
    {
        return itemsConsumableList;
    }

    public GameObject CreateEquipmentItem(Vector3 position, string itemName)
    {
        GameObject newItem = Instantiate(itemEquipmentGameObject, position, Quaternion.identity);
        newItem.GetComponent<ItemEquipmentScript>().SetName(itemName);

        return newItem;
    }
    
    public GameObject CreateConsumableItem(Vector3 position, string itemName)
    {
        GameObject newItem = Instantiate(itemConsumableGameObject, position, Quaternion.identity);
        newItem.GetComponent<ItemConsumableScript>().SetName(itemName);

        return newItem;
    }

    public bool IsItemPossessed(string name)
    {
        bool isPossesed = false;

        if (possessedItems.Contains(name))
            isPossesed = true;

        return isPossesed;
    }

    public void ApplyItemModifications(string itemName)
    {
        bool alreadyPossesed = IsItemPossessed(itemName);

        switch (itemName)
        {
            //stat modifying items
            case "Allumettes":
                PlayerScript.MyInstance.attackSpeed.MyMaxValue += 0.5f;
                break;

            case "Amulette du dragon":
                PlayerScript.MyInstance.attack.MyMaxValue += 1;
                break;

            case "Anneau du dragon":
                PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                PlayerScript.MyInstance.knockback.MyMaxValue += 2f;
                break;

            case "Anneau du forgeron":
                PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                PlayerScript.MyInstance.movementSpeed.MyMaxValue += 0.5f;
                PlayerScript.MyInstance.PlayerMaxLife += 10f;
                break;

            case "Bottes d'Hotavius":
                PlayerScript.MyInstance.movementSpeed.MyMaxValue += 1f;
                break;

            case "Cape de vampire":
                PlayerScript.MyInstance.invincibilityTime.MyMaxValue += 0.3f;
                break;

            case "Lampe à huile d'Hotavius":
                PlayerScript.MyInstance.PlayerMaxLife += 25f;
                break;

            case "Sauce piquante":
                break;

            default:
                break;
        }

        audio.clip = equipmentPickupClip;
        audio.Play();

        possessedItems.Add(itemName);
        
        InventoryManager.MyInstance.AddItem(itemName);
    }

    public string PlayerConsumableItem
    {
        get
        {
            return consumableItem;
        }

        set
        {
            consumableItem = value;
        }
    }

    public void UseConsumableItem()
    {      
        if (consumableItem != null)
        {
            GameObject[] enemies;

            switch (consumableItem)
            {               
                case "Potion de vitesse":
                    PlayerScript.MyInstance.attackSpeed.MyMaxValue += 0.75f;
                    PlayerScript.MyInstance.movementSpeed.MyMaxValue += 0.5f;
                    break;

                case "Potion de force":
                    PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                    break;

                case "Potion de lumière":
                    PlayerScript.MyInstance.PlayerMaxLife += 15f;
                    break;

                case "Parchemin de froid":
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        print(enemy.GetComponent<Character>().movementSpeed.MyCurrentValue);
                        enemy.GetComponent<Character>().movementSpeed.MyCurrentValue /= 2;
                        print(enemy.GetComponent<Character>().movementSpeed.MyCurrentValue);
                    }
                    break;
                    
                case "Parchemin de feu":
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        StartCoroutine(enemy.GetComponent<Character>().StartDamageOnTime(1f, 10, 1f));
                    }
                    break;

                case "Fruit étrange":
                    StartCoroutine(PlayerScript.MyInstance.GetComponent<PlayerScript>().StartNotLosingHealthWhenAttacking(5f));
                    break;

                case "Silex":
                    Vector2 playerPosition = PlayerScript.MyInstance.transform.position;

                    //if the player is on the bottom half, spawn the combustible toward the top, else toward the bot
                    float combustibleOffset = 0.8f;
                    if (playerPosition.y > 0)
                        combustibleOffset = -combustibleOffset;

                    GameObject combustible = Instantiate(combustibleGameObject, new Vector2(playerPosition.x, playerPosition.y + combustibleOffset), Quaternion.identity);
                    combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                    break;

                default:
                    break;
            }

            audio.clip = consumableTriggerClip;
            audio.Play();

            consumableItem = null;

            Image image = GameObject.Find("ConsumableItemUI").GetComponent<Image>();
            Color tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;
        }
    }
}

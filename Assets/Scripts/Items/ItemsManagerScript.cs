﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemsManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    public GameObject itemEquipmentGameObject;
    public GameObject itemConsumableGameObject;
    public GameObject player;

    private CanvasGroup canvasGroup;
    private Text itemText;


    private enum potionsEnum { none, vitesse, force, lumiere };
    private List<potionsEnum> potionUsed;

    private AudioSource audio;

    [SerializeField] private AudioClip equipmentPickupClip, consumableTriggerClip;

    [SerializeField] private GameObject combustibleGameObject;

    private static ItemsManagerScript instance;

    private List<string> possessedItems = new List<string>();
    private string consumableItem = null;

    private int numberOfItemsTotalUnlockUrbius = 4;
    private int numberOfItemsTotalUnlockIgeirus = 3;

    private bool bonusSetDragon = false;
    private bool bonusSetVampire = false;
    private bool bonusSetHotavius = false;

    //available items
    private List<string> itemsEquipmentList = new List<string> {
        "Allumettes",
        "Amulette du dragon",
        "Anneau du dragon",        
        "Bottes d'Hotavius",
        "Cape de vampire",
        "Lampe à huile d'Hotavius",
        "Sauce piquante",
        "Lance d'Hotavius",
        "Grimoire de boule de feu",
        "Carte d'Hotavius",
        "Plume du phoenix",        
        "Crocs de vampire",
        "Flamme éternelle",
        "Essence",
        "Roquette",
        "Poudre de métaux",
        "Anneau du forgeron"
    };

    private List<string> itemsConsumableList = new List<string> {
        "Parchemin de feu",
        "Parchemin de froid",
        "Fruit étrange",
        "Silex",
        "Potion de vitesse",
        "Potion de force",
        "Potion de lumière",
        "Potion de Urbius"
    };

    public List<string> EquipmentItems => itemsEquipmentList;
    
    public List<string> ConsumableItems => itemsConsumableList;

    public int NumberOfBaseConsumableItems => itemsConsumableList.Count - numberOfItemsTotalUnlockUrbius;
    public int NumberOfBaseEquipmentItems => itemsEquipmentList.Count - numberOfItemsTotalUnlockIgeirus;


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
        audio = GetComponent<AudioSource>();

        canvasGroup = popupPanel.GetComponent<CanvasGroup>();
        itemText = popupPanel.GetComponentInChildren<Text>();

        potionUsed = new List<potionsEnum>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UseConsumableItem();
    }

    public string SelectRandomItem(List<string> itemList)
    {
        int numberOfItemsToUnlock = 0;
        int numberOfItemsUnlocked = 0;

        if (itemList == itemsConsumableList)
        {
            numberOfItemsToUnlock = numberOfItemsTotalUnlockUrbius;
            numberOfItemsUnlocked = PlayerScript.MyInstance.MyUrbiusLevel;
        }

        else if (itemList == itemsEquipmentList)
        {
            numberOfItemsToUnlock = numberOfItemsTotalUnlockIgeirus;
            numberOfItemsUnlocked = PlayerScript.MyInstance.MyIgeirusLevel;
        }

        string randomItem = itemList[Random.Range(numberOfItemsToUnlock - numberOfItemsUnlocked, itemList.Count)];

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
                PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                break;

            case "Amulette du dragon":
                PlayerScript.MyInstance.attack.MyMaxValue += 1.5f;
                break;

            case "Anneau du dragon":
                PlayerScript.MyInstance.attack.MyMaxValue += 1f;
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
                PlayerScript.MyInstance.invincibilityTime.MyMaxValue += 0.5f;
                PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                break;

            case "Lampe à huile d'Hotavius":
                PlayerScript.MyInstance.PlayerMaxLife += 25f;
                PlayerScript.MyInstance.attackSpeed.MyMaxValue += 0.2f;
                break;

            case "Sauce piquante":
                PlayerScript.MyInstance.MyBossBonusDamage += 2f;
                break;

            case "Lance d'Hotavius":
                PlayerScript.MyInstance.IsProjectilePiercing = true;
                PlayerScript.MyInstance.range.MyMaxValue += 30;
                if (PlayerScript.MyInstance.attack.MyMaxValue < 3)
                    PlayerScript.MyInstance.attack.MyMaxValue -= 0.5f;
                else
                    PlayerScript.MyInstance.attack.MyMaxValue -= 1f;
                break;

            case "Grimoire de boule de feu":
                PlayerScript.MyInstance.IncreaseProjectileNumber();
                if (PlayerScript.MyInstance.attack.MyMaxValue < 2)
                    PlayerScript.MyInstance.attack.MyMaxValue -= 0.5f;
                else if (PlayerScript.MyInstance.attack.MyMaxValue < 4)
                    PlayerScript.MyInstance.attack.MyMaxValue -= 1f;
                else if (PlayerScript.MyInstance.attack.MyMaxValue < 6)
                    PlayerScript.MyInstance.attack.MyMaxValue -= 1.5f;
                else
                    PlayerScript.MyInstance.attack.MyMaxValue -= 2f;

                break;

            case "Carte d'Hotavius":
                DungeonFloorScript.MyInstance.ShowFullMap();
                PlayerScript.MyInstance.movementSpeed.MyMaxValue += 0.2f;
                break;

            case "Plume du phoenix":
                PlayerScript.MyInstance.IncreaseAdditionalLives();
                PlayerScript.MyInstance.PlayerMaxLife += 10f;
                break;

            case "Roquette":
                PlayerScript.MyInstance.MyRocketBonusDamage += 3f;
                break;

            case "Crocs de vampire":
                PlayerScript.MyInstance.MyHealOnSuccessiveHits += 15;
                PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                break;

            case "Flamme éternelle":
                PlayerScript.MyInstance.IncreaseCombustibleSpawnProbability();
                PlayerScript.MyInstance.PlayerMaxLife += 15f;
                break;

            case "Essence":
                PlayerScript.MyInstance.IncreaseBurningProjectileProbability();
                break;

            case "Poudre de métaux":
                PlayerScript.MyInstance.IncreaseColoredProjectilesLevel();
                break;

            default:
                break;
        }

        var mixer = Resources.Load("Sounds/AudioMixer") as AudioMixer;
        var volumeValue = .5f;
        var volume = !(mixer is null) && mixer.GetFloat("Volume", out volumeValue);

        if (volume)
        {
            audio.volume = 1-Math.Abs(volumeValue)/80;
        }

        audio.clip = equipmentPickupClip;
        audio.Play();

        possessedItems.Add(itemName);
        CheckForBonusSet();

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
                    potionUsed.Add(potionsEnum.vitesse);
                    break;

                case "Potion de force":
                    PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
                    potionUsed.Add(potionsEnum.force);
                    break;

                case "Potion de lumière":
                    PlayerScript.MyInstance.PlayerMaxLife += 15f;
                    PlayerScript.MyInstance.PlayerCurrentLife += 15f;
                    potionUsed.Add(potionsEnum.lumiere);
                    break;

                case "Potion de Urbius":
                    StartCoroutine(PlayerScript.MyInstance.GetComponent<PlayerScript>().StartInvincibility(10f));
                    break;

                case "Parchemin de froid":
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Character>().movementSpeed.MyCurrentValue /= 2;
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
                    float combustibleOffsetY = 0.8f;
                    if (playerPosition.y > 0)
                        combustibleOffsetY = -combustibleOffsetY;

                    //same for the x axis
                    float combustibleOffsetX = 0.8f;
                    if (transform.position.x > 0)
                        combustibleOffsetX = -combustibleOffsetX;

                    GameObject combustible = Instantiate(combustibleGameObject, new Vector2(playerPosition.x + combustibleOffsetX, playerPosition.y + combustibleOffsetY), Quaternion.identity);
                    combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                    break;

                default:
                    break;
            }
            
            var mixer = Resources.Load("Sounds/AudioMixer") as AudioMixer;
            var volumeValue = .5f;
            var volume = !(mixer is null) && mixer.GetFloat("Volume", out volumeValue);

            if (volume)
            {
                audio.volume = 1-Math.Abs(volumeValue)/80;
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

    public void RemovePotionEffect()
    {
        foreach (potionsEnum potionEffect in potionUsed)
        {
            switch (potionEffect)
            {
                case potionsEnum.vitesse:
                    PlayerScript.MyInstance.attackSpeed.MyMaxValue -= 0.75f;
                    PlayerScript.MyInstance.movementSpeed.MyMaxValue -= 0.5f;
                    break;

                case potionsEnum.force:
                    PlayerScript.MyInstance.attack.MyMaxValue -= 0.5f;
                    break;

                case potionsEnum.lumiere:
                    PlayerScript.MyInstance.PlayerMaxLife -= 15f;
                    break;
            }
        }

        potionUsed.Clear();
    }


    public IEnumerator FadingItemPopup(string itemName)
    {

        itemText.text = itemName;

        popupPanel.SetActive(true);

        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.05f;

            yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(2f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.05f;

            yield return new WaitForSeconds(.05f);
        }

        popupPanel.SetActive(false);
        yield return null;
    }

    private void CheckForBonusSet()
    {
        if (possessedItems.Contains("Amulette du dragon") 
            && possessedItems.Contains("Anneau du dragon")
            && bonusSetDragon == false)
        {
            bonusSetDragon = true;
            PlayerScript.MyInstance.attack.MyMaxValue += 1f;
        }

        if (bonusSetHotavius == false
            && possessedItems.Contains("Lampe à huile d'Hotavius")
            && possessedItems.Contains("Bottes d'Hotavius")
            && possessedItems.Contains("Lance d'Hotavius")
            && possessedItems.Contains("Carte d'Hotavius"))
        {
            bonusSetHotavius = true;
            CreateEquipmentItem(PlayerScript.MyInstance.transform.position, SelectRandomItem(GetItemsEquipmentList()));
        }

        if (bonusSetVampire == false
            && possessedItems.Contains("Cape de vampire")
            && possessedItems.Contains("Crocs de vampire"))
        {
            bonusSetVampire = true;
            PlayerScript.MyInstance.MyProjectileCost -= 0.5f;
        }
    }
}
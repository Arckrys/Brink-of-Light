using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManagerScript : MonoBehaviour
{
    public GameObject itemGameObject;
    public GameObject player;

    private PlayerScript playerScript;
    private AudioSource audio;

    //à mettre sur le personnage
    private List<string> possessedItems = new List<string>();

    //available items
    private List<string> itemsList = new List<string> {        
        "Allumettes",
        "Amulette du dragon",
        "Anneau du dragon",
        "Anneau du forgeron",
        "Bottes d'Hotavius",
        "Cape de vampire",
        "Lampe à huile d'Hotavius",
        "Sauce piquante"
    };

    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();

        audio = GetComponent<AudioSource>();

        ItemsTest();
    }

    private void ItemsTest()
    {
        CreateItem(new Vector3(4, 3, 0), "Allumettes");
        CreateItem(new Vector3(-2, 3, 0), SelectRandomItem());
        CreateItem(new Vector3(4, -3, 0), SelectRandomItem());
        CreateItem(new Vector3(-2, -3, 0), SelectRandomItem());
    }

    public string SelectRandomItem()
    {
        string randomItem = itemsList[Random.Range(0, itemsList.Count)];

        return randomItem;
    }

    public void CreateItem(Vector3 position, string itemName)
    {
        GameObject newItem = Instantiate(itemGameObject, position, Quaternion.identity);
        newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Items/Equipment/" + itemName);

        newItem.GetComponent<ItemScript>().SetName(itemName);
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
            case "Allumettes":
                playerScript.nextAttackDelay *= 0.5f;
                print("attack speed doublée");
                break;

            case "Amulette du dragon":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Anneau du dragon":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Anneau du forgeron":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Bottes d'Hotavius":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Cape du vampire":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Lampe à huile d'Hotavius":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            case "Sauce piquante":
                if (alreadyPossesed)
                {

                }

                else
                {

                }
                break;

            default:
                break;
        }

        audio.Play();

        possessedItems.Add(itemName);
    }
}

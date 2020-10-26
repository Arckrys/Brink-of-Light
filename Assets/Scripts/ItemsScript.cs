using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    public GameObject itemGameObject;

    private List<string> possessedItems = new List<string>();

    private List<string> itemsList = new List<string> {        
        "Allumettes",
        "Amulette du dragon",
        "Anneau du dragon",
        "Anneau du forgeron",
        "Bottes d'Hotavius",
        "Cape du vampire",
        "Lampe à huile d'Hotavius",
        "Sauce piquante"
    };

    public string SelectRandomItem()
    {
        string randomItem = itemsList[Random.Range(0, itemsList.Count)];

        return randomItem;
    }

    public void CreateItem(Vector3 position)
    {
        GameObject newItem = Instantiate(itemGameObject, position, Quaternion.identity);
        newItem.GetComponent<SpriteRenderer>();
    }

    public void ApplyItemModifications(string item)
    {
        switch (item)
        {
            case "Allumettes":
                if(possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Amulette du dragon":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Anneau du dragon":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Anneau du forgeron":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Bottes d'Hotavius":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Cape du vampire":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Lampe à huile d'Hotavius":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            case "Sauce piquante":
                if (possessedItems.Contains(item))
                {

                }

                else
                {

                }
                break;

            default:
                break;
        }

        possessedItems.Add(item);
    }
}

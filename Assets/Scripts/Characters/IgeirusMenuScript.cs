using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IgeirusMenuScript : MonoBehaviour
{
    [SerializeField] private Text price;
    
    [SerializeField] private Image item;
    
    [SerializeField] private Text description;
    
    private static IgeirusMenuScript _instance;
    
    public static IgeirusMenuScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<IgeirusMenuScript>();
            }

            return _instance;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        var playerLevel = PlayerScript.MyInstance.MyIgeirusLevel;
        price.text = playerLevel * 50 + 50 + " Ames";
        //Set Image -> Level
        //item.sprite = ...
        //Set Description -> Item
        //description.text = ...
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class SellerMenuScript : MonoBehaviour
{
    [SerializeField] private Text title;
    
    [SerializeField] private Text price;
    
    [SerializeField] private Image item;
    
    [SerializeField] private Text description;

    private NPCName name;

    public NPCName MyName
    {
        set => name = value;
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
        gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        var playerLevel = 0;
        
        switch (name)
        {
            case NPCName.Igeirus:
                playerLevel = PlayerScript.MyInstance.MyIgeirusLevel;
                title.text = "Igeirus le Forgeron";
                description.text = "Item description";
                break;
            case NPCName.Urbius:
                playerLevel = PlayerScript.MyInstance.MyUrbiusLevel;
                title.text = "Urbius l'Alchimiste";
                description.text = "Potion description";
                break;
            case NPCName.Razakus:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        price.text = playerLevel * 50 + 50 + " Ames";
        
        //Set Image -> Level
        //item.sprite = ...
    }
}

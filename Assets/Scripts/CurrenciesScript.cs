using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrenciesScript : MonoBehaviour
{
    
    [SerializeField] int soulsAmount, goldAmount;

    private static CurrenciesScript _instance;

    public static CurrenciesScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<CurrenciesScript>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setSoulsNumber(1505);
        setGoldValue(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSoulsNumber (int soulsNumber)
    {
        this.soulsAmount = soulsNumber;
        GameObject.Find("TextSoul").GetComponent<Text>().text = soulsAmount.ToString();
    }

    public int getSoulsNumber()
    {
        return soulsAmount;
    }

    public void addSouls(int soulsToAdd)
    {
        soulsAmount += soulsToAdd;
    }

    public void setGoldValue(int goldValue)
    {
        this.goldAmount = goldValue;
        GameObject.Find("TextGold").GetComponent<Text>().text = goldAmount.ToString();
    }

    public int getGoldValue()
    {
        return goldAmount;
    }

    public void addGold(int goldToAdd)
    {
        goldAmount += goldToAdd;
    }

    public bool purchaseForGold(int goldCost)
    {
        if (goldCost < this.goldAmount)
        {
            addGold(-goldCost);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool purchaseForSouls(int soulsCost)
    {
        if (soulsCost < this.soulsAmount)
        {
            addSouls(-soulsCost);
            return true;
        }
        else
        {
            return false;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        soulsAmount = 155;
        goldAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSoulsNumber (int soulsNumber)
    {
        this.soulsAmount = soulsNumber;
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

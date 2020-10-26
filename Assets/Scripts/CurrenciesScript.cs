using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrenciesScript : MonoBehaviour
{
    int soulsNumber, goldValue;

    // Start is called before the first frame update
    void Start()
    {
        soulsNumber = 0;
        goldValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSoulsNumber (int soulsNumber)
    {
        this.soulsNumber = soulsNumber;
    }

    public int getSoulsNumber()
    {
        return soulsNumber;
    }

    public void addSouls(int soulsToAdd)
    {
        soulsNumber += soulsToAdd;
    }

    public void setGoldValue(int goldValue)
    {
        this.goldValue = goldValue;
    }

    public int getGoldValue()
    {
        return goldValue;
    }

    public void addGold(int goldToAdd)
    {
        goldValue += goldToAdd;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrenciesScript : MonoBehaviour
{
    [SerializeField] private int soulsAmount, goldAmount;
    
    private int soulsUpdate, goldUpdate;

    private float baseIncrementTime = 0.7f;

    private static CurrenciesScript _instance;

    private bool isSoulsIncrementing = false; 
    private bool isGoldIncrementing = false; 

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
        soulsUpdate = 0;
        goldUpdate = 0;
        
        setSoulsNumber(1505);
        setGoldValue(25);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GoldIncrement()
    {
        isGoldIncrementing = true;
        float incrementTime = Math.Abs(baseIncrementTime / goldUpdate);

        while (goldUpdate != 0)
        {
            
            if (goldUpdate > 0)
            {
                goldAmount += 1;
                goldUpdate -= 1;
            }

            else
            {
                goldAmount -= 1;
                goldUpdate += 1;
            }
            
            GameObject.Find("TextGold").GetComponent<Text>().text = goldAmount.ToString();

            yield return new WaitForSeconds(incrementTime);
        }

        isGoldIncrementing = false;
    }
    
    private IEnumerator SoulIncrement()
    {
        isSoulsIncrementing = true;
        float incrementTime = Math.Abs(baseIncrementTime / soulsUpdate);

        while (soulsUpdate != 0)
        {

            if (soulsUpdate > 0)
            {
                soulsAmount += 1;
                soulsUpdate -= 1;
            }

            else
            {
                soulsAmount -= 1;
                soulsUpdate += 1;
            }

            GameObject.Find("TextSoul").GetComponent<Text>().text = soulsAmount.ToString();

            yield return new WaitForSeconds(incrementTime);
        }

        isSoulsIncrementing = false;
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
        soulsUpdate += soulsToAdd;
        StartCoroutine(SoulIncrement());
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
        goldUpdate += goldToAdd;
        StartCoroutine(GoldIncrement());
    }

    public bool purchaseForGold(int goldCost)
    {
        if (goldCost <= this.goldAmount && !isGoldIncrementing)
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
        if (soulsCost < this.soulsAmount && !isSoulsIncrementing)
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

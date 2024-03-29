﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurrenciesScript : MonoBehaviour
{
    [SerializeField] private int soulsAmount, goldAmount;

    private int soulsToPrint, goldToPrint;

    private float baseIncrementTime = 0.7f;

    private static CurrenciesScript _instance;

    private bool isSoulsIncrementing; 
    private bool isGoldIncrementing;

    private Coroutine goldIncrementCoroutine;
    private Coroutine soulsIncrementCoroutine;
    
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
    private void Start()
    {
        SetGoldValue(goldAmount);
        SetSoulsNumber(soulsAmount);
    }

    // Starts the animation that incremented gold
    private IEnumerator GoldIncrement()
    {
        var incrementTime = Math.Abs(baseIncrementTime / Math.Abs(goldAmount - goldToPrint));

        while (goldToPrint != goldAmount)
        {
            goldToPrint += goldToPrint < goldAmount ? 1 : -1;

            GameObject.Find("TextGold").GetComponent<Text>().text = goldToPrint.ToString();

            yield return new WaitForSeconds(incrementTime);
        }
    }
    
    // Starts the animation that incremented souls
    private IEnumerator SoulIncrement()
    {
        var incrementTime = Math.Abs(baseIncrementTime / Math.Abs(soulsAmount - soulsToPrint));

        while (soulsToPrint != soulsAmount)
        {
            soulsToPrint += soulsToPrint < soulsAmount ? 1 : -1;

            GameObject.Find("TextSoul").GetComponent<Text>().text = soulsToPrint.ToString();

            yield return new WaitForSeconds(incrementTime);
        }
    }

    public void SetSoulsNumber (int soulsNumber)
    {
        soulsAmount = soulsNumber;
        soulsToPrint = soulsAmount;
        GameObject.Find("TextSoul").GetComponent<Text>().text = soulsToPrint.ToString();
    }

    public int GetSoulsNumber()
    {
        return soulsAmount;
    }

    public void AddSouls(int soulsToAdd)
    {
        soulsAmount += soulsToAdd;
        
        SaveSystem.SaveCurrencies();
        
        if (soulsIncrementCoroutine != null)
        {
            StopCoroutine(soulsIncrementCoroutine);
        }
        
        soulsIncrementCoroutine = StartCoroutine(SoulIncrement());
    }

    public void SetGoldValue(int goldValue)
    {
        goldAmount = goldValue;
        goldToPrint = goldAmount;
        GameObject.Find("TextGold").GetComponent<Text>().text = goldToPrint.ToString();
    }

    public int GetGoldValue()
    {
        return goldAmount;
    }

    public void AddGold(int goldToAdd)
    {
        goldAmount += goldToAdd;
        
        SaveSystem.SaveCurrencies();

        if (goldIncrementCoroutine != null)
        {
            StopCoroutine(goldIncrementCoroutine);    
        }
        
        goldIncrementCoroutine = StartCoroutine(GoldIncrement());
    }

    public bool PurchaseForGold(int goldCost)
    {
        if (goldCost > goldAmount) return false;
        
        AddGold(-goldCost);
        return true;
    }

    public bool PurchaseForSouls(int soulsCost)
    {
        if (soulsCost > soulsAmount) return false;
        
        AddSouls(-soulsCost);
        return true;
    }
}

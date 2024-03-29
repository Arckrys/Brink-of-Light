﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RazakusMenuScript : MonoBehaviour
{

    private Dictionary<string, double[]> RazakusData = new Dictionary<string, double[]>();
    private readonly Dictionary<string, dynamic> StatsAndTexts = new Dictionary<string, dynamic>();
    private static RazakusMenuScript _instance;

    private bool hasBeenInit = false;

    private string[] statNames = new string[] { "Attaque", "Vie", "Portee", "Vitesse", "VitesseAtk", "ChanceCrit", "DegatsCrit", "Recul" };

    // Text displayed on purchase buttons
    [SerializeField] private Text AttaqueButton;
    [SerializeField] private Text VieButton;
    [SerializeField] private Text PorteeButton;
    [SerializeField] private Text VitesseButton;
    [SerializeField] private Text VitesseAtkButton;
    [SerializeField] private Text ChanceCritButton;
    [SerializeField] private Text DegatsCritButton;
    [SerializeField] private Text ReculButton;

    // Text by the stat name, shows current stat's value
    [SerializeField] private Text AttaqueStat;
    [SerializeField] private Text VieStat;
    [SerializeField] private Text PorteeStat;
    [SerializeField] private Text VitesseStat;
    [SerializeField] private Text VitesseAtkStat;
    [SerializeField] private Text ChanceCritStat;
    [SerializeField] private Text DegatsCritStat;
    [SerializeField] private Text ReculStat;

    // Text next to the button, shows upgrade's value
    [SerializeField] private Text AttaqueUpgrade;
    [SerializeField] private Text VieUpgrade;
    [SerializeField] private Text PorteeUpgrade;
    [SerializeField] private Text VitesseUpgrade;
    [SerializeField] private Text VitesseAtkUpgrade;
    [SerializeField] private Text ChanceCritUpgrade;
    [SerializeField] private Text DegatsCritUpgrade;
    [SerializeField] private Text ReculUpgrade;
    
    private PlayerScript player;


    public void Start()
    {
        if (hasBeenInit)
            return;

        // Data : Name AmountBought/PriceIncreasePerUpgrade/InitialPrice/UpgradeAmount
        RazakusData.Add("Attaque",      new double[] { 0, 50, 50, 0.5 });
        RazakusData.Add("Vie",          new double[] { 0, 50, 50, 10 });
        RazakusData.Add("Portee",       new double[] { 0, 50, 50, 10 });
        RazakusData.Add("Vitesse",      new double[] { 0, 50, 50, 0.5 });
        RazakusData.Add("VitesseAtk",   new double[] { 0, 50, 50, 0.2 });
        RazakusData.Add("ChanceCrit",   new double[] { 0, 50, 100, 5 });
        RazakusData.Add("DegatsCrit",   new double[] { 0, 50, 100, 0.2 });
        RazakusData.Add("Recul",        new double[] { 0, 50, 100, 1 });

        // Dictionnary used to link stat's name with dynamic objects needed
        // Data : Name    StatValue/ButtonText/StatText/UpgradeText
        StatsAndTexts.Add("Attaque",      System.Tuple.Create(PlayerScript.MyInstance.initAttack, AttaqueButton, AttaqueStat, AttaqueUpgrade));
        StatsAndTexts.Add("Vie",          System.Tuple.Create(PlayerScript.MyInstance.initLife, VieButton, VieStat, VieUpgrade));
        StatsAndTexts.Add("Portee",       System.Tuple.Create(PlayerScript.MyInstance.initRange, PorteeButton, PorteeStat, PorteeUpgrade));
        StatsAndTexts.Add("Vitesse",      System.Tuple.Create(PlayerScript.MyInstance.initMovementSpeed, VitesseButton, VitesseStat, VitesseUpgrade));
        StatsAndTexts.Add("VitesseAtk",   System.Tuple.Create(PlayerScript.MyInstance.initAttackSpeed, VitesseAtkButton, VitesseAtkStat, VitesseAtkUpgrade));
        StatsAndTexts.Add("ChanceCrit",   System.Tuple.Create(PlayerScript.MyInstance.initCritChance, ChanceCritButton, ChanceCritStat, ChanceCritUpgrade));
        StatsAndTexts.Add("DegatsCrit",   System.Tuple.Create(PlayerScript.MyInstance.initCritDamage, DegatsCritButton, DegatsCritStat, DegatsCritUpgrade));
        StatsAndTexts.Add("Recul",        System.Tuple.Create(PlayerScript.MyInstance.initKnockback, ReculButton, ReculStat, ReculUpgrade));

        player = PlayerScript.MyInstance;
        
        InitUI();

        gameObject.SetActive(false);

        hasBeenInit = true;
    }

    public static RazakusMenuScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<RazakusMenuScript>();
            }

            return _instance;
        }
    }

    // Returns an array containing the amount of purchases for each stat
    public double[] GetRazakusPurchases()
    {
        double[] purchases = new double[statNames.Length];
        
        for(int i = 0; i < statNames.Length; i++)
        {
            purchases[i] = RazakusData[statNames[i]][0];
        }
        return purchases;
    }

    // Updates Razakus menu given purchases data
    public void LoadRazakusData(double[] savedData)
    {
        if (!hasBeenInit) 
        {
            Start();
        }

        for (int i = 0; i < statNames.Length; i++)
        {
            if(savedData != null)
                RazakusData[statNames[i]][0] = savedData[i];
        }
        
        InitUI();
    }

    // Reads the RazakusData dictionnary to initialize the menu
    public void InitUI()
    {
        UpdateUI("Attaque", player.initAttack, true);
        UpdateUI("Vie", player.initLife, true);
        UpdateUI("Portee", player.initRange, true);
        UpdateUI("Vitesse", player.initMovementSpeed, true);
        UpdateUI("VitesseAtk", player.initAttackSpeed, true);
        UpdateUI("ChanceCrit", player.initCritChance, true);
        UpdateUI("DegatsCrit", player.initCritDamage, true);
        UpdateUI("Recul", player.initKnockback, true);
    }
    
    // Updates one of the menu's line given the stat name and value 
    private void UpdateUI(string statName, float statValue, bool init = false)
    {
        if (!init)
        {
            RazakusData[statName][0] += 1;
        }
        
        StatsAndTexts[statName].Item2.text = RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2] + " Ames";
        StatsAndTexts[statName].Item3.text = System.Math.Round(statValue, 1).ToString(CultureInfo.InvariantCulture);
        StatsAndTexts[statName].Item4.text = "+" + RazakusData[statName][3];
    }

    // Computes the price of the upgrade bought and decreases souls
    private bool PurchaseSouls(string statName)
    {
        return CurrenciesScript.MyInstance.PurchaseForSouls((int)(RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]));
    }

    // The following methods are linked to ingame buttons to update stats and save new data
    public void OnHealthPressed()
    {
        const string statName = "Vie";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.PlayerMaxLife += value;
            player.PlayerCurrentLife += value;
            UpdateUI(statName, player.initLife += value);
            GameManager.MyInstance.SaveGame();
        }
    }
    public void OnAttackPressed()
    {
        const string statName = "Attaque";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.attack.MyMaxValue += value;
            UpdateUI(statName, player.initAttack += value);
            GameManager.MyInstance.SaveGame();
        }
    }
    public void OnRangePressed()
    {
        const string statName = "Portee";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.range.MyMaxValue += value;
            UpdateUI(statName, player.initRange += value);
            GameManager.MyInstance.SaveGame();
        }
    }
    public void OnCritChancePressed()
    {
        const string statName = "ChanceCrit";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.critChance.MyMaxValue += value;
            UpdateUI(statName, player.initCritChance += value);
            GameManager.MyInstance.SaveGame();
        }
    }
    public void OnSpeedPressed()
    {
        const string statName = "Vitesse";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.movementSpeed.MyMaxValue += value;
            UpdateUI(statName, player.initMovementSpeed += value);
            GameManager.MyInstance.SaveGame();
        }
    }

    public void OnAtkSpeedPressed()
    {
        const string statName = "VitesseAtk";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.attackSpeed.MyMaxValue += value;
            UpdateUI(statName, player.initAttackSpeed += value);
            GameManager.MyInstance.SaveGame();
        }
    }

    public void OnCritDmgPressed()
    {
        const string statName = "DegatsCrit";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.critDamage.MyMaxValue += value;
            UpdateUI(statName, player.initCritDamage += value);
            GameManager.MyInstance.SaveGame();
        }
    }
   
    public void OnKnockbackPressed()
    {
        const string statName = "Recul";
        if (PurchaseSouls(statName))
        {
            var value = (float) RazakusData[statName][3];
            player.knockback.MyMaxValue += value;
            UpdateUI(statName, player.initKnockback += value);
            GameManager.MyInstance.SaveGame();
        }
    }
}

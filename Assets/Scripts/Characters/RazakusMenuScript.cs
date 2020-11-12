using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RazakusMenuScript : MonoBehaviour
{

    private Dictionary<string, double[]> RazakusData = new Dictionary<string, double[]>();
    private readonly Dictionary<string, dynamic> StatsAndTexts = new Dictionary<string, dynamic>();
    private static RazakusMenuScript _instance;

    //private string[] statNames = new string[] { "Attaque", "Vie", "Portee", "Vitesse", "VitesseAtk", "ChanceCrit", "DegatsCrit", "Recul" };

    [SerializeField] private Text AttaqueButton;
    [SerializeField] private Text VieButton;
    [SerializeField] private Text PorteeButton;
    [SerializeField] private Text VitesseButton;
    [SerializeField] private Text VitesseAtkButton;
    [SerializeField] private Text ChanceCritButton;
    [SerializeField] private Text DegatsCritButton;
    [SerializeField] private Text ReculButton;

    [SerializeField] private Text AttaqueStat;
    [SerializeField] private Text VieStat;
    [SerializeField] private Text PorteeStat;
    [SerializeField] private Text VitesseStat;
    [SerializeField] private Text VitesseAtkStat;
    [SerializeField] private Text ChanceCritStat;
    [SerializeField] private Text DegatsCritStat;
    [SerializeField] private Text ReculStat;

    [SerializeField] private Text AttaqueUpgrade;
    [SerializeField] private Text VieUpgrade;
    [SerializeField] private Text PorteeUpgrade;
    [SerializeField] private Text VitesseUpgrade;
    [SerializeField] private Text VitesseAtkUpgrade;
    [SerializeField] private Text ChanceCritUpgrade;
    [SerializeField] private Text DegatsCritUpgrade;
    [SerializeField] private Text ReculUpgrade;


    public void Start()
    {   // Data : Name AmountBought/PriceIncreasePerUpgrade/InitialPrice/UpgradeAmount
        RazakusData.Add("Attaque",      new double[] { 0, 50, 50, 0.5 });
        RazakusData.Add("Vie",          new double[] { 0, 50, 50, 5 });
        RazakusData.Add("Portee",       new double[] { 0, 50, 50, 10 });
        RazakusData.Add("Vitesse",      new double[] { 0, 50, 50, 1 });
        RazakusData.Add("VitesseAtk",   new double[] { 0, 50, 50, -0.1 });
        RazakusData.Add("ChanceCrit",   new double[] { 0, 50, 100, 1 });
        RazakusData.Add("DegatsCrit",   new double[] { 0, 50, 100, 0.1 });
        RazakusData.Add("Recul",        new double[] { 0, 50, 100, 1 });

        StatsAndTexts.Add("Attaque",      System.Tuple.Create(PlayerScript.MyInstance.attack, AttaqueButton, AttaqueStat, AttaqueUpgrade));
        StatsAndTexts.Add("Vie",          System.Tuple.Create(PlayerScript.MyInstance.life, VieButton, VieStat, VieUpgrade));
        StatsAndTexts.Add("Portee",       System.Tuple.Create(PlayerScript.MyInstance.range, PorteeButton, PorteeStat, PorteeUpgrade));
        StatsAndTexts.Add("Vitesse",      System.Tuple.Create(PlayerScript.MyInstance.movementSpeed, VitesseButton, VitesseStat, VitesseUpgrade));
        StatsAndTexts.Add("VitesseAtk",   System.Tuple.Create(PlayerScript.MyInstance.attackSpeed, VitesseAtkButton, VitesseAtkStat, VitesseAtkUpgrade));
        StatsAndTexts.Add("ChanceCrit",   System.Tuple.Create(PlayerScript.MyInstance.critChance, ChanceCritButton, ChanceCritStat, ChanceCritUpgrade));
        StatsAndTexts.Add("DegatsCrit",   System.Tuple.Create(PlayerScript.MyInstance.critDamage, DegatsCritButton, DegatsCritStat, DegatsCritUpgrade));
        StatsAndTexts.Add("Recul",        System.Tuple.Create(PlayerScript.MyInstance.knockback, ReculButton, ReculStat, ReculUpgrade));

        InitUI();

        gameObject.SetActive(false);
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

    public void InitUI()
    {
        UpdateUI("Attaque", true);
        UpdateUI("Vie", true);
        UpdateUI("Portee", true);
        UpdateUI("Vitesse", true);
        UpdateUI("VitesseAtk", true);
        UpdateUI("ChanceCrit", true);
        UpdateUI("DegatsCrit", true);
        UpdateUI("Recul", true);
    }
    
    private void UpdateUI(string statName, bool init = false)
    {
        if (!init)
        {
            RazakusData[statName][0] += 1;
            StatsAndTexts[statName].Item1.MyMaxValue += RazakusData[statName][3];
        }

        StatsAndTexts[statName].Item2.text = (RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]).ToString() + " Ames";
        StatsAndTexts[statName].Item3.text = System.Math.Round(StatsAndTexts[statName].Item1.MyMaxValue, 1).ToString();
        StatsAndTexts[statName].Item4.text = "+" + RazakusData[statName][3];
    }

    private bool PurchaseSouls(string statName)
    {
        return CurrenciesScript.MyInstance.purchaseForSouls((int)(RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]));
    }

    public void OnHealthPressed()
    {
        if (PurchaseSouls("Vie"))
        {
            UpdateUI("Vie");
        }
        
    }
    public void OnAttackPressed()
    {
        if (PurchaseSouls("Attaque"))
        {
            UpdateUI("Attaque");
        }
    }
    public void OnRangePressed()
    {
        if (PurchaseSouls("Portee"))
        {
            UpdateUI("Portee");
        }
    }
    public void OnCritChancePressed()
    {
        if (PurchaseSouls("ChanceCrit"))
        {
            UpdateUI("ChanceCrit");
        }
    }
    public void OnSpeedPressed()
    {
        if (PurchaseSouls("Vitesse"))
        {
            UpdateUI("Vitesse");
        }
    }

    public void OnAtkSpeedPressed()
    {
        if (PurchaseSouls("VitesseAtk"))
        {
            UpdateUI("VitesseAtk"); 
        }
    }

    public void OnCritDmgPressed()
    {
        if (PurchaseSouls("DegatsCrit"))
        {
            UpdateUI("DegatsCrit");
        }
    }
   
    public void OnKnockbackPressed()
    {
        if (PurchaseSouls("Recul"))
        {
            UpdateUI("Recul");
        }
    }
}

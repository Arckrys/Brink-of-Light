using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RazakusMenuScript : MonoBehaviour
{

    private Dictionary<string, double[]> RazakusData = new Dictionary<string, double[]>();
    private Dictionary<string, dynamic> PlayerStats = new Dictionary<string, dynamic>();

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

    private bool statTextsInitialized = false;


    public void Start()
    {   // Data : Name AmountBought/PriceIncreasePerUpgrade/InitialPrice/UpgradeAmount
        RazakusData.Add("Attaque",      new double[] { 0, 50, 50, 0.5 });
        RazakusData.Add("Vie",          new double[] { 0, 50, 50, 5 });
        RazakusData.Add("Portee",       new double[] { 0, 50, 50, 10 });
        RazakusData.Add("Vitesse",      new double[] { 0, 50, 50, 1 });
        RazakusData.Add("VitesseAtk",   new double[] { 0, 50, 50, 0.1 });
        RazakusData.Add("ChanceCrit",   new double[] { 0, 50, 100, 0.1 });
        RazakusData.Add("DegatsCrit",   new double[] { 0, 50, 100, 0.1 });
        RazakusData.Add("Recul",        new double[] { 0, 50, 100, 1 });

        PlayerStats.Add("Attaque", System.Tuple.Create(PlayerScript.MyInstance.attack, AttaqueButton, AttaqueStat));
        PlayerStats.Add("Vie", System.Tuple.Create(PlayerScript.MyInstance.life, VieButton, VieStat));
        PlayerStats.Add("Portee", System.Tuple.Create(PlayerScript.MyInstance.range, PorteeButton, PorteeStat));
        PlayerStats.Add("Vitesse", System.Tuple.Create(PlayerScript.MyInstance.movementSpeed, VitesseButton, VitesseStat));
        PlayerStats.Add("VitesseAtk", System.Tuple.Create(PlayerScript.MyInstance.attackSpeed, VitesseAtkButton, VitesseAtkStat));
        PlayerStats.Add("ChanceCrit", System.Tuple.Create(PlayerScript.MyInstance.critChance, ChanceCritButton, ChanceCritStat));
        PlayerStats.Add("DegatsCrit", System.Tuple.Create(PlayerScript.MyInstance.critDamage, DegatsCritButton, DegatsCritStat));
        PlayerStats.Add("Recul", System.Tuple.Create(PlayerScript.MyInstance.knockback, ReculButton, ReculStat));

        InitUI();
    }

    private void Update()
    {
        if (!statTextsInitialized)
        {
            InitUI();
            statTextsInitialized = true;
        }
            
    }

    private void InitUI()
    {
        UpdateUI(AttaqueButton, AttaqueStat, "Attaque", true);
        UpdateUI(VieButton, VieStat, "Vie", true);
        UpdateUI(PorteeButton, PorteeStat, "Portee", true);
        UpdateUI(VitesseButton, VitesseStat, "Vitesse", true);
        UpdateUI(VitesseAtkButton, VitesseAtkStat, "VitesseAtk", true);
        UpdateUI(ChanceCritButton, ChanceCritStat, "ChanceCrit", true);
        UpdateUI(DegatsCritButton, DegatsCritStat, "DegatsCrit", true);
        UpdateUI(ReculButton, ReculStat, "Recul", true);
    }

    private void UpdateUI(Text buttonText, string statName, bool init = false)
    {
        if(!init)
            RazakusData[statName][0] += 1;
        buttonText.text = (RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]).ToString() + " Ames";
    }
    
    private void UpdateUI(Text buttonText, Text statText, string statName, bool init = false)
    {
        if (!init)
            RazakusData[statName][0] += 1;

        PlayerStats[statName].Item2.text = (RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]).ToString() + " Ames";
        PlayerStats[statName].Item3.text = PlayerStats[statName].Item1.MyMaxValue+ " " + RazakusData[statName][3];

    }

    private bool PurchaseSouls(string statName)
    {
        return CurrenciesScript.MyInstance.purchaseForSouls((int)(RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]));
    }

    public void OnHealthPressed()
    {
        if (PurchaseSouls("Vie"))
        {
            PlayerScript.MyInstance.PlayerMaxLife += 5f;
            UpdateUI(VieButton, "Vie");
        }
        
    }
    public void OnAttackPressed()
    {
        if (PurchaseSouls("Attaque"))
        {
            PlayerScript.MyInstance.attack.MyMaxValue += 0.5f;
            UpdateUI(AttaqueButton, AttaqueStat, "Attaque");
        }
    }
    public void OnRangePressed()
    {
        if (PurchaseSouls("Portee"))
        {
            PlayerScript.MyInstance.range.MyMaxValue += 10f;
            UpdateUI(PorteeButton, "Portee");
        }
    }
    public void OnCritChancePressed()
    {
        if (PurchaseSouls("ChanceCrit"))
        {
            PlayerScript.MyInstance.critChance.MyMaxValue += 0.1f;
            UpdateUI(ChanceCritButton, "ChanceCrit");
        }
    }
    public void OnSpeedPressed()
    {
        if (PurchaseSouls("Vitesse"))
        {
            PlayerScript.MyInstance.movementSpeed.MyMaxValue += 1f;
            UpdateUI(VitesseButton, "Vitesse");
        }
    }

    public void OnAtkSpeedPressed()
    {
        if (PurchaseSouls("VitesseAtk"))
        {
            PlayerScript.MyInstance.attackSpeed.MyMaxValue -= 0.1f;
            UpdateUI(VitesseAtkButton, "VitesseAtk"); 
        }
    }

    public void OnCritDmgPressed()
    {
        if (PurchaseSouls("DegatsCrit"))
        {
            PlayerScript.MyInstance.critDamage.MyMaxValue += 0.1f;
            UpdateUI(DegatsCritButton, "DegatsCrit");
        }
    }
   
    public void OnKnockbackPressed()
    {
        if (PurchaseSouls("Recul"))
        {
            PlayerScript.MyInstance.knockback.MyMaxValue += 1f;
            UpdateUI(ReculButton, "Recul");
        }
    }
}

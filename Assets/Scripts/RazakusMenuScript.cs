using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RazakusMenuScript : MonoBehaviour
{

    private Dictionary<string, int[]> RazakusData = new Dictionary<string, int[]>();
    private string[] statNames = new string[] { "Attaque", "Vie", "Portee", "Vitesse", "VitesseAtk", "ChanceCrit", "Recul" };

    [SerializeField] private Text Attaque;
    [SerializeField] private Text Vie;
    [SerializeField] private Text Portee;
    [SerializeField] private Text Vitesse;
    [SerializeField] private Text VitesseAtk;
    [SerializeField] private Text ChanceCrit;
    //[SerializeField] private Text DegatsCrit;
    [SerializeField] private Text Recul;

    public void Start()
    {   // Data : Name AmountBought/PriceIncreasePerUpgrade/InitialPrice
        RazakusData.Add("Attaque",      new int[] { 0, 50, 50 });
        RazakusData.Add("Vie",          new int[] { 0, 50, 50 });
        RazakusData.Add("Portee",       new int[] { 0, 50, 50 });
        RazakusData.Add("Vitesse",      new int[] { 0, 50, 50 });
        RazakusData.Add("VitesseAtk",   new int[] { 0, 50, 50 });
        RazakusData.Add("ChanceCrit",   new int[] { 0, 50, 100 });
        RazakusData.Add("Recul",        new int[] { 0, 50, 100 });

        InitUI();
    }

    private void InitUI()
    {
        UpdateUI(Attaque, "Attaque", true);
        UpdateUI(Vie, "Vie", true);
        UpdateUI(Portee, "Portee", true);
        UpdateUI(Vitesse, "Vitesse", true);
        UpdateUI(VitesseAtk, "VitesseAtk", true);
        UpdateUI(ChanceCrit, "ChanceCrit", true);
        UpdateUI(Recul, "Recul", true);
    }

    private void UpdateUI(Text text, string statName, bool init = false)
    {
        if(!init)
            RazakusData[statName][0] += 1;
        text.text = (RazakusData[statName][0] * RazakusData[statName][1] + RazakusData[statName][2]).ToString() + " Ames";
    }

    public void OnHealthPressed()
    {
        PlayerScript.MyInstance.LifeMaxValue += 10f;
        UpdateUI(Vie, "Vie");
    }
    public void OnAttackPressed()
    {
        PlayerScript.MyInstance.AttackMaxValue += 1f;
        UpdateUI(Attaque, "Attaque");
    }
    public void OnRangePressed()
    {
        PlayerScript.MyInstance.RangeMaxValue += 1f;
        UpdateUI(Portee, "Portee");
    }
    public void OnCritChancePressed()
    {
        PlayerScript.MyInstance.CritChanceMaxValue += 1f;
        UpdateUI(ChanceCrit, "ChanceCrit");
    }
    public void OnSpeedPressed()
    {
        PlayerScript.MyInstance.MovementSpeedMaxValue += 5f;
        UpdateUI(Vitesse, "Vitesse");
    }

    public void OnAtkSpeedPressed()
    {
        PlayerScript.MyInstance.AttackSpeedMaxValue += 1f;
        UpdateUI(VitesseAtk, "VitesseAtk");
    }

   /* public void OnCritDmgPressed()
    {
        PlayerScript.MyInstance.CritDMGMaxValue += 10f;
        RazakusData["DegatsCrit"][0] += 1;
        UpdateUI(Vitesse);
    }*/
    public void OnKnockbackPressed()
    {
        PlayerScript.MyInstance.MovementSpeedMaxValue += 10f;
        UpdateUI(Recul, "Recul"); ;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private RazakusMenuScript razakusScript;
    private PlayerScript playerScript;
    private CurrenciesScript currencyScript;

    private float[] playerStatsData;
    private int[] currenciesData;

    private void Start()
    {
        razakusScript = RazakusMenuScript.MyInstance;
        playerScript = PlayerScript.MyInstance;
        currencyScript = CurrenciesScript.MyInstance;
    }

    public static GameManager MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }
    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            razakusScript.LoadRazakusData(data.GetRazakusPurchases());

            playerStatsData = data.GetPlayerStatMaxValues();
            playerScript.attack.MyMaxValue = playerStatsData[0];
            playerScript.life.MyMaxValue = playerStatsData[1];
            playerScript.range.MyMaxValue = playerStatsData[2];
            playerScript.movementSpeed.MyMaxValue = playerStatsData[3];
            playerScript.attackSpeed.MyMaxValue = playerStatsData[4];
            playerScript.critChance.MyMaxValue = playerStatsData[5];
            playerScript.critDamage.MyMaxValue = playerStatsData[6];
            playerScript.knockback.MyMaxValue = playerStatsData[7];

            currenciesData = data.GetCurrencies();

            currencyScript.setGoldValue(currenciesData[0]);
            currencyScript.setSoulsNumber(currenciesData[1]);


            // Save items bought
        }
    }
}

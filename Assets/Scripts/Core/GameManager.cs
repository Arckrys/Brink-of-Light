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
    
    [SerializeField] private GameObject closeDoors;
    [SerializeField] private GameObject openDoors;

    private void Start()
    {
        razakusScript = RazakusMenuScript.MyInstance;
        playerScript = PlayerScript.MyInstance;
        currencyScript = CurrenciesScript.MyInstance;
    }
    
    void Update()
    {
        UpdateDoorState();
    }

    private void UpdateDoorState()
    {
        if (closeDoors != null && openDoors != null)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                closeDoors.SetActive(false);
                openDoors.SetActive(true);
            }
            else
            {
                closeDoors.SetActive(true);
                openDoors.SetActive(false);
            }
        }

        else
        {
            FindRoomDoors();
        }
    }

    //get the doors grids of the current room
    public void FindRoomDoors()
    {
        closeDoors = GameObject.Find("BorderGridClose");
        openDoors = GameObject.Find("BorderGridOpen");
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

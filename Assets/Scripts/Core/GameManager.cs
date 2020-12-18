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
    [SerializeField] private GameObject chestPrefab;

    private bool enemiesAllKilled;

    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuGraphics;
    [SerializeField] private GameObject menuAudio;
    [SerializeField] private GameObject menuMort;
    private bool inPause;

    public bool PauseState => inPause;

    private void Start()
    {
        enemiesAllKilled = false;
        razakusScript = RazakusMenuScript.MyInstance;
        playerScript = PlayerScript.MyInstance;
        currencyScript = CurrenciesScript.MyInstance;
        
        menuPause.SetActive(false);
        menuGraphics.SetActive(false);
        menuAudio.SetActive(false);
        menuMort.SetActive(false);
        inPause = false;

        if (PlayerPrefs.GetInt("Restart") != 1) return;

        GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>().alpha = 1;
        
        StartCoroutine(WaitForScene());
            
        PlayerPrefs.SetInt("Restart", 0);
    }

    void Update()
    {
        UpdateDoorState();
        
        GetPauseKey();
    }
    
    private IEnumerator WaitForScene()
    {
        yield return new WaitForSeconds(1);
        
        StartCoroutine(CanvasTransitionScript.MyInstance.FadeIn(null, true));
    }

    // Starts pause menu when pressing ECHAP key (and fix time)
    private void GetPauseKey()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (!PlayerScript.MyInstance.GetIsInMenu())
            EditPauseState(!inPause);
    }

    public void EditPauseState(bool state)
    {
        // Switch between menus
        if (!state && (menuGraphics.activeSelf || menuAudio.activeSelf))
        {
            menuGraphics.SetActive(state);
            menuAudio.SetActive(state);
            menuPause.SetActive(!state);
        }
        // Fix time if menu is opened
        else
        {
            inPause = state;
            menuPause.SetActive(state);
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    // Switch to graphic menu
    public void SetGraphicMenu(bool state)
    {
        menuPause.SetActive(!state);
        menuGraphics.SetActive(state);
    }

    // Switch to audio menu
    public void SetAudioMenu(bool state)
    {
        menuPause.SetActive(!state);
        menuAudio.SetActive(state);
    }

    // Launch death menu
    public void SetDeathMenu(bool state)
    {
        MusicManager.MyInstance.SetCurrentMusic("death");
        
        menuMort.SetActive(state);
    }

    private void UpdateDoorState()
    {
        // When all enemies are killed and there is an exit door
        if (closeDoors != null && openDoors != null && !enemiesAllKilled)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                // Open all doors
                closeDoors.SetActive(false);
                openDoors.SetActive(true);
                enemiesAllKilled = true;

                if (DungeonFloorScript.MyInstance.GetCurrentNode().GetRoomType() == FloorNode.roomTypeEnum.regular)
                {
                    // Instantiate chest in room
                    int randomInt = Random.Range(0, 15);
                    if (randomInt == 0)
                    {
                        GameObject chest = Instantiate(chestPrefab);
                        chest.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                    }
                }
            }
            else
            {
                // Close all doors
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

    public void ResetRoomDoors()
    {
        closeDoors = null;
        openDoors = null;
        enemiesAllKilled = false;
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

    // Load a backup
    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            if (data.GetRazakusPurchases() != null)
            {
                razakusScript.LoadRazakusData(data.GetRazakusPurchases());
            }

            playerStatsData = data.GetPlayerStatMaxValues();
            playerScript.initAttack = playerStatsData[0];
            playerScript.initLife = playerStatsData[1];
            playerScript.initRange = playerStatsData[2];
            playerScript.initMovementSpeed= playerStatsData[3];
            playerScript.initAttackSpeed = playerStatsData[4];
            playerScript.initCritChance = playerStatsData[5];
            playerScript.initCritDamage = playerStatsData[6];
            playerScript.initKnockback = playerStatsData[7];

            playerScript.attack.MyMaxValue = playerScript.initAttack;
            playerScript.PlayerCurrentLife = playerScript.initLife;
            playerScript.PlayerMaxLife = playerScript.initLife;
            playerScript.range.MyMaxValue = playerScript.initRange;
            playerScript.movementSpeed.MyMaxValue = playerScript.initMovementSpeed;
            playerScript.attackSpeed.MyMaxValue = playerScript.initAttackSpeed;
            playerScript.critChance.MyMaxValue = playerScript.initCritChance;
            playerScript.critDamage.MyMaxValue = playerScript.initCritDamage;
            playerScript.knockback.MyMaxValue = playerScript.initKnockback;

            playerScript.MyIgeirusLevel = data.GetIgeirusLevel();
            playerScript.MyUrbiusLevel = data.GetUrbiusLevel();

            currenciesData = data.GetCurrencies();

            currencyScript.SetGoldValue(currenciesData[0]);
            currencyScript.SetSoulsNumber(currenciesData[1]);
        }
    }
}

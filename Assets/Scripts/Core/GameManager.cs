using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private RazakusMenuScript razakusScript;

    private void Start()
    {
        razakusScript = RazakusMenuScript.MyInstance;
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
        razakusScript.LoadRazakusData(data.getRazakusPurchases());
    }
}

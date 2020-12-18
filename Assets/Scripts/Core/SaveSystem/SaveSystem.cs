using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //static string path = Application.persistentDataPath + "/BrinkOfLight.save";
    static string path = "./BrinkOfLight.save";

    // Save game data
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData();

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved");
    }

    // Save only currencies
    public static void SaveCurrencies()
    {
        var load = LoadGame();
        if (load == null) return;
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        load.SetCurrencies(CurrenciesScript.MyInstance.GetGoldValue(), CurrenciesScript.MyInstance.GetSoulsNumber());
            
        formatter.Serialize(stream, load);
        stream.Close();
        Debug.Log("Saved Currencies");
    }

    public static bool DoesSaveExist()
    {
        return File.Exists(path);
    }

    // Load a backup
    public static GameData LoadGame()
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = (GameData) formatter.Deserialize(stream);
            stream.Close();
            Debug.Log("Loaded at " + path);
            return data;
        }
        else
        {
            Debug.Log("Save File not Found in " + path);
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (!File.Exists(path)) return;
        
        var saveFile = new FileInfo(path);
        saveFile.Delete();
    }
}
